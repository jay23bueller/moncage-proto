using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    [SerializeField]
    private Camera[] _secondaryCameras;
    private bool _enableRotation = false;

    private void Start()
    {
        SetLookAtTarget();
    }

    private void Update()
    {
        CheckForRightClick();
        HandleZoom();
        HandleMovementAndRotation();
    }


    private void CheckForRightClick()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Input.mousePosition;
            Ray mouseInputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
           
            RaycastHit hit;
            Physics.Raycast(mouseInputRay, out hit, 10f);

            if(hit.collider != null)
            {
                Vector3 normalInverted = -hit.normal;
                RaycastHit secondHit = new RaycastHit();
                Debug.DrawLine(transform.position, hit.point, Color.red, 4f);
                if (hit.collider.CompareTag("FirstView"))
                {
                    HitThroughCamera(0,hit,normalInverted,ref secondHit, mouseInputRay);
                }

                if(hit.collider.CompareTag("SecondView"))
                {
                    HitThroughCamera(1, hit, normalInverted, ref secondHit, mouseInputRay);
                }

                if(hit.collider.CompareTag("ThirdView"))
                {
                    HitThroughCamera(2, hit, normalInverted, ref secondHit, mouseInputRay);
                }

                if(secondHit.collider != null)
                {
                    Debug.Log(secondHit.collider.gameObject.name);
                }
                
            }
        }
    }

    private void HitThroughCamera(int cameraId, RaycastHit hit, Vector3 invertedNormal, ref RaycastHit secondHit, Ray mouseInputRay)
    {
        Ray mouseC = _secondaryCameras[cameraId].ViewportPointToRay(hit.textureCoord);
        
        Debug.DrawLine(mouseC.origin, mouseC.GetPoint(10f), Color.red, 5f);
        Physics.Raycast(mouseC, out secondHit, 40f, LayerMask.GetMask("Selectable"));

        if(secondHit.collider != null)
        {
            Color c = secondHit.collider.GetComponent<Renderer>().material.color;
            secondHit.collider.GetComponent<Renderer>().material.color = c == Color.white ? Color.red : Color.white;
        }
    }

    private void SetLookAtTarget()
    {
        _camera.transform.LookAt(_targetTransform);
    }


    private void HandleMovementAndRotation()
    {
        if (Input.GetMouseButtonDown(0))
            _enableRotation = true;

        if(Input.GetMouseButtonUp(0))
            _enableRotation = false;


        if (_enableRotation)
        {
            
            float mouseXDeltaSpeed = Input.GetAxis("Mouse X") * _mouseYSensitivity * Time.deltaTime;
            transform.RotateAround(_targetTransform.position + Vector3.up * (transform.position.y - _targetTransform.position.y), Vector3.up, mouseXDeltaSpeed);

            float mouseYDeltaSpeed = -Input.GetAxis("Mouse Y") * _mouseXSensitivity * Time.deltaTime;
            transform.RotateAround(_targetTransform.position, transform.right, mouseYDeltaSpeed);


        }
        foreach (var secondaryCamera in _secondaryCameras)
            secondaryCamera.transform.rotation = transform.rotation;
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
