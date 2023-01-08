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
    public float defaultInterpolationTime = 0.8f;
    public float startInterpolationTime = 0.8f;
    private bool isFirst = true;
    private float interpolationTime;
    private float elapsedTime;
    private bool shouldMove = false;
    private GameObject targetObject;


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
        interpolationTime = startInterpolationTime;
        elapsedTime = interpolationTime;
        MoveTo(startTarget);
        isFirst = false;
    }

    void Update()
    {
        /*var interpolationRatio = (float)elapsedTime / interpolationTime;
        Vector3 interpolatedPosition = Vector3.Slerp(startPos, targetPos, interpolationRatio);
        Quaternion interpolatedRotation = Quaternion.Slerp(startRot, targetRot, interpolationRatio);
        elapsedTime += Time.deltaTime;
        transform.position = interpolatedPosition;
        transform.rotation = interpolatedRotation;
        */
        // transform.LookAt(targetObject.transform);
    }

    public void MoveTo(GameObject target)
    {
        if (!isFirst)
            interpolationTime = defaultInterpolationTime;
        float lookAngle = 82.5f;
        if (target.transform.rotation != Quaternion.Euler(0, 0, 0))
            lookAngle = 90f;
        targetPos = target.transform.position + target.transform.up * DEFAULT_HEIGHT;
        targetRot = target.transform.rotation * Quaternion.Euler(new Vector3(lookAngle, 0, 0));
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPos, "time", interpolationTime, "easetype", iTween.EaseType.easeInOutSine));
        iTween.RotateTo(this.gameObject, iTween.Hash("rotation", targetRot.eulerAngles, "time", interpolationTime, "easetype", iTween.EaseType.easeInOutSine));
        /*startPos = transform.position;
        // targetPos = target.transform.position;
        targetPos = target.transform.position + target.transform.up * DEFAULT_HEIGHT;
        startRot = transform.rotation;
        float lookAngle = 82.5f;
        if (target.transform.rotation != Quaternion.Euler(0, 0, 0))
            lookAngle = 90f;
        targetRot = target.transform.rotation * Quaternion.Euler(new Vector3(lookAngle, 0, 0));
        // targetPos.y = DEFAULT_HEIGHT;
        elapsedTime = 0;
        // targetObject = target;
        */
    }
}
