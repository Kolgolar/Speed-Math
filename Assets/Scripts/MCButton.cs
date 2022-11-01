using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MCButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool boolData;
    public static Action<bool> onClicked;


    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke(boolData);
    }
}