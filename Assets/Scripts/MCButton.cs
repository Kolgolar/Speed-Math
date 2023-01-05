using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MCButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool boolData;
    public static Action<bool> onClicked;
    public static bool canBePressed = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canBePressed)
            onClicked?.Invoke(boolData);
    }
}
