using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class RestartGameButton : MonoBehaviour, IPointerClickHandler
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        onClicked?.Invoke();
    }
}
