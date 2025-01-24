using System.Numerics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Animations;

public class TestMonobehavior : MonoBehaviour
{
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Application.isPlaying)
        {
            print("Game is Playing");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
