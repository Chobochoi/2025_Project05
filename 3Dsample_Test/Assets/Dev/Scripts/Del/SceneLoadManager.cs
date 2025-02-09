using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private string sceneName; // �ε��� �� �̸�
    [SerializeField] private InputAction activateSceneAction; // Input Action
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject loadingScreen;
    private bool hasExecuted = false;

    private AsyncOperation asyncOperation;

    void OnEnable()
    {
        activateSceneAction.Enable(); // Input Action Ȱ��ȭ
        activateSceneAction.performed += OnActivateScene; // Ű �Է� �̺�Ʈ ����
    }

    void OnDisable()
    {
        activateSceneAction.performed -= OnActivateScene; // �̺�Ʈ ����
        activateSceneAction.Disable(); // Input Action ��Ȱ��ȭ
    }

    private void OnActivateScene(InputAction.CallbackContext context)
    {
        // F1 Ű �Է� ���� �� �� Ȱ��ȭ
        if (asyncOperation != null && !asyncOperation.allowSceneActivation)
        {
            Debug.Log("F1 Ű �Է�: �� Ȱ��ȭ!");
            asyncOperation.allowSceneActivation = true;
        }
    }

    void Start()
    {
        // �񵿱� �ε� ����
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // �� �񵿱� �ε� ����
        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // �ε� ����� Ȯ��
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            if (progress == 1.0f && !hasExecuted)
            {
                Debug.Log($"�ε� �����: {progress * 100}%");
                hasExecuted = true;
            }            
            yield return null;
        }
    }
}