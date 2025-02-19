using UnityEngine;
using Valve.VR;

public class PlayerController_SteamVR : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 이동 속도
    public Transform cameraRig; // SteamVR Camera Rig
    public Transform headTransform; // HMD 위치 (플레이어 바라보는 방향)

    public SteamVR_Action_Vector2 moveAction; // 컨트롤러의 스틱 입력

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.axis; // 스틱 입력 받기

        if (input.magnitude > 0.1f) // 입력 값이 일정 이상이면 이동
        {
            Vector3 direction = new Vector3(input.x, 0, input.y); // X,Z 방향 이동
            Vector3 move = headTransform.TransformDirection(direction); // HMD 방향 기준 이동

            cameraRig.position += move * moveSpeed * Time.deltaTime; // 이동 적용
        }
    }
}
