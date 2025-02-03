# 2025_Project05
## Unity6 Sample for Test
### 해보고 싶은 것
#### RestAPI 호출
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
