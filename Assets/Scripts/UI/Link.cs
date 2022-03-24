using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Link : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public string url;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK");
#if !UNITY_EDITOR
		openWindow(url);
#else
        Application.OpenURL(url);
#endif
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
