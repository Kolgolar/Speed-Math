using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject startTarget;
    const float DEFAULT_HEIGHT = 4.8f;
    private Vector3 targetPos, startPos;
    private Quaternion startRot, targetRot;
    private static float interpolationTime = 0.5f;
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
       MoveTo(startTarget);
    }

    void Update()
    {
        if (elapsedTime < interpolationTime)
        {
            float interpolationRatio = (float)elapsedTime / interpolationTime;
            Vector3 interpolatedPosition = Vector3.Slerp(startPos, targetPos, interpolationRatio);
            Quaternion interpolatedRotation = Quaternion.Slerp(startRot, targetRot, interpolationRatio);
            elapsedTime += Time.deltaTime;
            transform.position = interpolatedPosition;
            transform.rotation = interpolatedRotation;
        }
    }

    public void MoveTo(GameObject target)
    {
        startPos = transform.position;
        targetPos = target.transform.position;
        startRot = transform.rotation;
        targetRot = Quaternion.Euler(new Vector3(75, 0, 0));
        targetPos.y = DEFAULT_HEIGHT;
        elapsedTime = 0;
    }
}
