using UnityEngine;

namespace UnityNote
{
    // �ε�ȭ�� ���� �� �ε��̹��� ȸ���� ����
    public class RotateEffect : MonoBehaviour
    {
        [SerializeField] float rotateSpeed = 100;
        [SerializeField] bool isPlay = true;

        public void Update()
        {
            if (isPlay) return;
            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }
}
