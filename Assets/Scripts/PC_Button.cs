using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PC_Button : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool boolData;
    public static Action<bool> onClicked;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke(boolData);
    }
}
