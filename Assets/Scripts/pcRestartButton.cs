using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class pcRestartButton : MonoBehaviour, IPointerClickHandler
{
    public static Action onClicked;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke();
    }
}
