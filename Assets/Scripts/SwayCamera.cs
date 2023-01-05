using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SwayCamera : MonoBehaviour
{
    [SerializeField] float intensity = 1.0f; // Скорость раскачки
    [SerializeField] float amplitude = 0.1f; // Высота раскачки

    Vector3 nextSwayVector;
    Vector3 nextSwayPosition;
    Vector3 startLocalPosition;
    private bool isMoving = false;
    public bool IsMoving {set{if (!isMoving == value) UpdatePoses(); isMoving = value;}}

    void Start()
    {
        
    }

    void Update()
    {
        // if (Player.isMove) // Если игрок движется
        if (!isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextSwayPosition, intensity * Time.deltaTime);

            if (Vector3.SqrMagnitude(transform.localPosition - nextSwayPosition) < 0.01f)
            {
                nextSwayVector = -nextSwayVector;

                nextSwayPosition = startLocalPosition + nextSwayVector;
            }
        }
        else
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startLocalPosition, intensity * Time.deltaTime);
    }

    void UpdatePoses()
    {
        nextSwayVector = Vector3.up * amplitude;
        nextSwayPosition = transform.localPosition + nextSwayVector;
        startLocalPosition = transform.localPosition;
    }
}