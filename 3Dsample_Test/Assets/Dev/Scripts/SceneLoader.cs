using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum SceneNames { Intro = 0, Lobby = 1 };

namespace UnityNote
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        // Scene�� Load�ϱ� ���� ������
        [SerializeField] GameObject loadingScreen;  // �ε� ȭ��
        [SerializeField] Image loadingBackground;   // �ε� �̹���
        [SerializeField] Sprite[] loadingSprites;   // ��� �̹��� ���
        [SerializeField] Slider loadingProgress;    // �ε� ������ ��
        [SerializeField] TextMeshProUGUI textProgress;  // �ε� ���൵ �ؽ�Ʈ

        private WaitForSeconds waitChangeDelay;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                waitChangeDelay = new WaitForSeconds(0.5f);

                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadScene(string name)
        {
            int index = Random.Range(0, loadingSprites.Length); // �ε� �� Sprite�� Sprite ���� ������ �����ϰ� ������ �ϱ� ����
            loadingBackground.sprite = loadingSprites[index];   // �ε� �� ����̹��� ������ ����
            loadingProgress.value = 0;                          // ���� �� 0�ʺ��� ����
            loadingScreen.SetActive(true);                      // �ε� ����

            StartCoroutine(LoadSceneAsync(name));
        }

        public void LoadScene(SceneNames name)
        {
            LoadScene(name.ToString());
        }

        private IEnumerator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperaction = SceneManager.LoadSceneAsync(name);

            // Scene�� �ҷ��ö����� �ݺ�
            while (asyncOperaction.isDone == false)
            {
                loadingProgress.value = asyncOperaction.progress;
                textProgress.text = $"{Mathf.RoundToInt(asyncOperaction.progress * 100)}";
                yield return null;
            }

            yield return waitChangeDelay;

            loadingScreen.SetActive(false);
        }
    }
}


