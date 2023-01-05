using System.Collections;
using UnityEngine;
using UnityEngine.UI;
 
public class Timer: MonoBehaviour
{
    [SerializeField] private float time;
 
    private float timeLeft = 0f;
 
    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimeText();
            yield return null;
        }
    }
 
    private void Start()
    {
        timeLeft = time;
        StartCoroutine(StartTimer());
    }
 
    private void UpdateTimeText()
    {
        if (timeLeft < 0)
            timeLeft = 0;
 
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
    }
}