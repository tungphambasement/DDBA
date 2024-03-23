using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offSet = new Vector3(0f,0f,-10f);
    [SerializeField] private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    void Awake(){
        _offSet = transform.position-target.position;
    }
    void LateUpdate()
    {
        Vector3 targetPosition = target.position + _offSet;
        transform.position = Vector3.SmoothDamp(transform.position,targetPosition, ref velocity, smoothTime);
    }
}
