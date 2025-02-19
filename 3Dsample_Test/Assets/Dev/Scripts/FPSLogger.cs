using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLogger : MonoBehaviour
{
    private void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        Debug.Log($"FPS: {fps:F2}");
    }
}
