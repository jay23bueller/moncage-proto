using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private float _mouseScrollSpeed = 5f;
    [SerializeField]
    private float _mouseYSensitivity = 15f;
    [SerializeField]
    private float _mouseXSensitivity = 60f;
    [SerializeField]
    private Camera _camera;
    private bool _enableYRotation = false;
    private bool _enableXRotation = false;


    private void Start()
    {
        SetLookAtTarget();
    }

    private void Update()
    {

        HandleZoom();
        HandleMovement();
    }

    private void SetLookAtTarget()
    {
        _camera.transform.LookAt(_targetTransform);
    }


    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
            _enableYRotation = true;

        if(Input.GetMouseButtonUp(0))
            _enableYRotation = false;


        if (Input.GetMouseButtonDown(1))
            _enableXRotation = true;

        if (Input.GetMouseButtonUp(1))
            _enableXRotation = false;

        if (_enableYRotation)
        {
            
            float mouseXDeltaSpeed = Input.GetAxis("Mouse X") * _mouseYSensitivity * Time.deltaTime;

            transform.RotateAround(_targetTransform.position + Vector3.up * (transform.position.y - _targetTransform.position.y), Vector3.up, mouseXDeltaSpeed);



        } else if(_enableXRotation)
        {
            float mouseYDeltaSpeed = -Input.GetAxis("Mouse Y") * _mouseYSensitivity * Time.deltaTime;
            Debug.Log(Input.GetAxis("Mouse X"));
            transform.RotateAround(_targetTransform.position, transform.right, mouseYDeltaSpeed);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_targetTransform.position + Vector3.up * (transform.position.y - _targetTransform.position.y), .2f);     
    }

    private void HandleZoom()
    {

        transform.position += transform.forward * Input.mouseScrollDelta.y * _mouseScrollSpeed;
    }
}