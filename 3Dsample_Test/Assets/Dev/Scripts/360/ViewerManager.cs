using UnityEngine;

public class ViewerManager : MonoBehaviour
{
    public delegate void SetCameraPositionHandler(Vector3 cam_Position, Vector3 cam_Direction);
    public static SetCameraPositionHandler pos_SetCameraPosition;

    // �ش� Camera�� ���� ����� Camera ����ϱ�.
    [SerializeField] public Transform trans_Camera;

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
