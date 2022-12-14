using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GMButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int gameMode;
    public static Action<int> onClicked;


    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke(gameMode);
    }
}
