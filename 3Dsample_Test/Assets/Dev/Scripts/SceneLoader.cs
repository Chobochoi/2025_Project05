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

        // Scene을 Load하기 위한 정보들
        [SerializeField] GameObject loadingScreen;  // 로딩 화면
        [SerializeField] Image loadingBackground;   // 로딩 이미지
        [SerializeField] Sprite[] loadingSprites;   // 배경 이미지 목록
        [SerializeField] Slider loadingProgress;    // 로딩 게이지 바
        [SerializeField] TextMeshProUGUI textProgress;  // 로딩 진행도 텍스트

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
            int index = Random.Range(0, loadingSprites.Length); // 로딩 시 Sprite가 Sprite 개수 내에서 랜덤하게 나오게 하기 위함
            loadingBackground.sprite = loadingSprites[index];   // 로딩 시 배경이미지 지정을 위함
            loadingProgress.value = 0;                          // 시작 시 0초부터 시작
            loadingScreen.SetActive(true);                      // 로딩 시작

            StartCoroutine(LoadSceneAsync(name));
        }

        public void LoadScene(SceneNames name)
        {
            LoadScene(name.ToString());
        }

        private IEnumerator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperaction = SceneManager.LoadSceneAsync(name);

            // Scene을 불러올때까지 반복
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


