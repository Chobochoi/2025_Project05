using UnityEngine;

public class SceneCheckManager : MonoBehaviour
{
    private bool isChecked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChecked)
        {
            Debug.Log("Scene_Open");
            isChecked = true;
        }
    }
}
