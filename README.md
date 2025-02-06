# 2025_Project05
## Unity6 Sample for Test
### 해보고 싶은 것
#### RestAPI 호출
#### 로딩씬 구현
#### 공공데이터 표출 및 갱신
#### Canvas 관리 및 하위 GameObject 관리
#### 추가 예정

#### 추후 테스트 및 커밋 예정 코드
```
// Unity
// 25.02.03
// 아직 직접 만들어보지 않고 가상으로 작성하였기에, 버그 및 실질적으로 운용에 대해선 아직 미지수임.
using UnityEngine;

public class ViewerManager : MonoBehaviour
{
	public delegate void SetCameraPositionHandler(Vector3 cam_Position, Vector3 cam_Direction);
	public static SetCameraPositionHandler pos_SetCameraPosition;
	
	// 해당 Camera는 내가 사용할 Camera 등록하기.
	public Transform trans_Camera;
	
	private void OnEnable()
	{
        pos_SetCameraPosition += SetCamera;
	}

    private void OnDisable()
    {
        pos_SetCameraPosition -= SetCamera;
    }
	
    public void SetCamera(Vector3 cam_Position, Vector3 cam_Direction)
    {
        trans_Camera.position = cam_Position;
        trans_Camera.LookAt(cam_Direction);
    }
}
```

```
// Unity
// 25.02.03
// 아직 직접 만들어보지 않고 가상으로 작성하였기에, 버그 및 실질적으로 운용에 대해선 아직 미지수임.
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HotSpot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject thisPanorama;
    public GameObject targetPanorama;

    public void Update()
    {
        transform.Rotate(0, 0.5f, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerEnter();
        OnPointerExit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(0.08f, 0.08f, 0.08f), 0.3f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        trasform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0,3f);
    }

    public void OnHotSpotTransition()
    {
        SetSkyBox();
    }

    private void SetSkyBox()
    {
        if(ViewerManager.pos_SetCameraPosition != null)
        {
            ViewerManager.pos_SetCameraPosition(targetPanorama.transform.position, thisPanorama.transform.poisition);
            targetPanorama.gameObject.SetActivate(true);
            thisPanorama.gameObject.SetActivate(false);     
        }
    }
}
```
```
// Unity
// 25.02.06
// 아직 직접 만들어보지 않고 가상으로 작성하였기에, 버그 및 실질적으로 운용에 대해선 아직 미지수임.
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// 씬의 갯수에 따른 enum 
public enum SceneNames {Intro = 0, Lobby}

namespace UnityNote
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        
        // Loading에 관련된 변수들 SerializeField
        [SerializeField] 
        private GameObject loadingScreen;   // 로딩 화면

        [SerializeField]
        private Image loadingBackground;     // 로딩 이미지

        [SerializeField]
        private sprite[] loadingSprites;        // 배경 이미지 목록
        
        [SerializeField]
        private Slider loadingProgress;     // 로딩 프로그래스바

        [SerializeField]
        private TextMeshProUGUI textProgress;       // 로딩 진행도 텍스트

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
            int index = Random.Range(0, loadingSprites);
            loadingBackground.sprite = loadingSprites[index];
            loadingProgress.value = 0;
            loadingScreen.SetActivate(true);

            StartCoroutine(LoadSceneAsync(name));
        }

        public void LoadScene(SceneNames name)
        {
            LoadScene(name.ToString());
        }

        private IEnumulator LoadSceneAsync(string name)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

            // Case 1. 로딩 시간이 너무 짧아 특정 시간동안 Scene 보여주기 위함.
            float percent = 0;
            float loadingTime = 2.5f;
            while( percent < 1f)
            {
                percent += Time.deltaTime / loadingTime;
                loadingProgress.value = percent;
                textProgress.text = $"{Mathf.RoundToInt(percent * 100)}";

                yeild return null;
            }                  
                  
            // Case 2. 비동기 작업(씬 불러오기)이 완료될 때까지 반복
            while(asyncOperation.isDone == false)
            {
                // 비동기 작업의 진행 상황 (0.0 ~ 1.0)
                loadingProgress.value = asyncOperation.progress;
                textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";

                yeild return null;
            }
            
            yeild return waitChangeDelay;

            loadingScreen.SetActivate(false);
        }
           
    }
}
```
```
// Unity
// 25.02.06
// 아직 직접 만들어보지 않고 가상으로 작성하였기에, 버그 및 실질적으로 운용에 대해선 아직 미지수임.
using UnityEngine;

namespace UnityNote
{
    // 오브젝트 회전을 위한 Class
    public class RotateEffect : MonoBehaviour
    {
        [SerializeField]
        private float rotateSpeed = 100;

        [SerializeField]
        private bool isPlay = true;

        private void Update()
        {
            if (isPlay) return;

            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }
}
```
```
// Unity
// 25.02.06
// 아직 직접 만들어보지 않고 가상으로 작성하였기에, 버그 및 실질적으로 운용에 대해선 아직 미지수임.
using UnityEngine;

// SceneController를 위함
public class IntroSceneController : MonoBehaviour
{
    public void GameStartEvent()
    {
        UnityNote.SceneLoader.Instance.LoadScene(SceneNames.Lobby);
    }

    public void GameExitEvent()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
```
