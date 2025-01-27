using UnityEngine;
using System;
using System.IO;

public class PathManager : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void SplitPath(string fullPath)
    {
        string directory = Path.GetDirectoryName(fullPath);
        string fileName = Path.GetFileName(fullPath);

        Debug.Log($"Directory: {directory}");
        Debug.Log($"File Name: {fileName}");
    }

    private void Update()
    {
        SplitPath("Assets/Textures/brick_wall.png");
    }

}

