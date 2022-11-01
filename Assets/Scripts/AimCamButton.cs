using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class AimCamButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject target;
    public static Action<GameObject> onClicked;


    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke(target);
    }
}
