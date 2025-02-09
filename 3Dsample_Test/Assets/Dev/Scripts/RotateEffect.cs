using UnityEngine;

namespace UnityNote
{
    // 로딩화면 시작 시 로딩이미지 회전을 위함
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
