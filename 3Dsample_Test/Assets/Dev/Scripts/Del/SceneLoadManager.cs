using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private string sceneName; // 로드할 씬 이름
    [SerializeField] private InputAction activateSceneAction; // Input Action
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject loadingScreen;
    private bool hasExecuted = false;

    private AsyncOperation asyncOperation;

    void OnEnable()
    {
        activateSceneAction.Enable(); // Input Action 활성화
        activateSceneAction.performed += OnActivateScene; // 키 입력 이벤트 연결
    }

    void OnDisable()
    {
        activateSceneAction.performed -= OnActivateScene; // 이벤트 해제
        activateSceneAction.Disable(); // Input Action 비활성화
    }

    private void OnActivateScene(InputAction.CallbackContext context)
    {
        // F1 키 입력 감지 후 씬 활성화
        if (asyncOperation != null && !asyncOperation.allowSceneActivation)
        {
            Debug.Log("F1 키 입력: 씬 활성화!");
            asyncOperation.allowSceneActivation = true;
        }
    }

    void Start()
    {
        // 비동기 로드 시작
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // 씬 비동기 로딩 시작
        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // 로딩 진행률 확인
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            if (progress == 1.0f && !hasExecuted)
            {
                Debug.Log($"로딩 진행률: {progress * 100}%");
                hasExecuted = true;
            }            
            yield return null;
        }
    }
}