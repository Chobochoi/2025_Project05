using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IMG_Test : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] 
    public Image myImage;
    public Texture myTexture;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked Image");
        
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
