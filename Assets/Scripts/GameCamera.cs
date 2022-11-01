using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    const float DEFAULT_HEIGHT = 4.8f;
    private Vector3 targetPos;
    private Vector3 startPos;
    private static float interpolationTime = 1f;
    private float elapsedTime = interpolationTime;


   private void OnEnable() 
    {
        AimCamButton.onClicked += MoveTo;
    }

    private void OnDisable() 
    {
        AimCamButton.onClicked -= MoveTo;
    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, DEFAULT_HEIGHT, transform.position.z);
    }

    void Update()
    {
        if (elapsedTime < interpolationTime)
        {
            float interpolationRatio = (float)elapsedTime / interpolationTime;
            Vector3 interpolatedPosition = Vector3.Slerp(startPos, targetPos, interpolationRatio);
            elapsedTime += Time.deltaTime;
            transform.position = interpolatedPosition;
        }
    }

    void MoveTo(GameObject target)
    {
        startPos = transform.position;
        targetPos = target.transform.position;
        targetPos.y = DEFAULT_HEIGHT;
        elapsedTime = 0;
    }
}
