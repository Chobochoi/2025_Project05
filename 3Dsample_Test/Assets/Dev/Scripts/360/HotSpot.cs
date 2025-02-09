using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;

public class HotSpot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public GameObject thisPanorama;
    [SerializeField] public GameObject targetPanorama;

    public void Update()
    {
        transform.Rotate(0, 0.5f, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerEnter(eventData);
        OnHotSpotTransition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(0.08f, 0.08f, 0.08f), 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 3f);
    }

    public void OnHotSpotTransition()
    {
        SetSkyBox();
    }

    private void SetSkyBox()
    {
        if (ViewerManager.pos_SetCameraPosition != null)
        {
            ViewerManager.pos_SetCameraPosition(targetPanorama.transform.position, thisPanorama.transform.position);
            targetPanorama.gameObject.SetActive(true);
            thisPanorama.gameObject.SetActive(false);
        }
    }
}