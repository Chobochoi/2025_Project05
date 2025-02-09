using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInputManager : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        // 커서를 기본 상태로 설정 (초기화)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log($"Camera : {playerCamera.transform}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            HandleClick();
            Debug.Log("Mouse left Clicked");
            DetectClickedObject();
        }

        // ESC 키를 누르면 마우스를 다시 표시 (게임에서 나갈 수 있도록)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void DetectClickedObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
        {
            Debug.Log($"클릭된 오브젝트: {hit.collider.gameObject.name}");
        }
        else
        {
            Debug.Log("어떤 오브젝트도 클릭되지 않음");
        }
    }

    private void HandleClick()
    {
        // UI 버튼 클릭 감지
        if (IsPointerOverUI())
        {
            Debug.Log("UI 버튼 클릭됨!");
            return;
        }

        // 23D 오브젝트 클릭 감지
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
        {
            Debug.Log($"3D 오브젝트 클릭: {hit.collider.gameObject.name}, Hit 위치 : {hit.point}");
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
