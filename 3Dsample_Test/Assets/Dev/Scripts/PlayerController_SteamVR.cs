using UnityEngine;
using Valve.VR;

public class PlayerController_SteamVR : MonoBehaviour
{
    public float moveSpeed = 2.0f; // �̵� �ӵ�
    public Transform cameraRig; // SteamVR Camera Rig
    public Transform headTransform; // HMD ��ġ (�÷��̾� �ٶ󺸴� ����)

    public SteamVR_Action_Vector2 moveAction; // ��Ʈ�ѷ��� ��ƽ �Է�

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.axis; // ��ƽ �Է� �ޱ�

        if (input.magnitude > 0.1f) // �Է� ���� ���� �̻��̸� �̵�
        {
            Vector3 direction = new Vector3(input.x, 0, input.y); // X,Z ���� �̵�
            Vector3 move = headTransform.TransformDirection(direction); // HMD ���� ���� �̵�

            cameraRig.position += move * moveSpeed * Time.deltaTime; // �̵� ����
        }
    }
}
