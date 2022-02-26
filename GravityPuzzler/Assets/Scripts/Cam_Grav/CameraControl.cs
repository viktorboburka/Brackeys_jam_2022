using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;
    [SerializeField]
    private float _rotationSpeed = 0.1f;
    private float _rotationChangedTime = 0.0f;

    [SerializeField]
    private bool _playIntro = false;
    float zoom;
    float targetZoom = 1f;
    Camera cam;
    float zoomTime = Mathf.Infinity;
    float zoomSpeed = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        _currentRotation = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        _targetRotation = _currentRotation;
        _rotationSpeed = 0.1f;
        cam = Camera.main;
        zoom = cam.orthographicSize;
        Debug.Log("zoom: " + zoom);
    }

    // Update is called once per frame
    void Update()
    {
        smoothRotate();
        if (_playIntro) {
            zoomIn();
        }
    }

    void zoomIn() {
        if (zoomTime == Mathf.Infinity) {
            zoomTime = Time.timeSinceLevelLoad;
        }
        if (zoom != targetZoom) {
            Debug.Log("zoom: " + zoom + " target zoom: " + targetZoom + "zoomspeed * ...: " + zoomSpeed * (Time.timeSinceLevelLoad - zoomTime));
            float newZoom = Mathf.Lerp(zoom, targetZoom, zoomSpeed * (Time.timeSinceLevelLoad - zoomTime));
            //Debug.Log("zooming to: " + zoom + " target: " + targetZoom);
            cam.orthographicSize = newZoom;
        }
    }

    public void setTargetRotation(GravityControl.GravityDirection target) {
        switch (target) {
            case GravityControl.GravityDirection.UP:
                _targetRotation = new Vector3 (0.0f, 0.0f, 180.0f);
                break;
            case GravityControl.GravityDirection.DOWN:
                _targetRotation = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case GravityControl.GravityDirection.LEFT:
                _targetRotation = new Vector3(0.0f, 0.0f, -90f);
                break;
            case GravityControl.GravityDirection.RIGHT:
                _targetRotation = new Vector3(0.0f, 0.0f, 90.0f);
                break;
            default:
                _targetRotation = new Vector3(0.0f, 0.0f, 0.0f);
                break;
        }
        _rotationChangedTime = Time.timeSinceLevelLoad;
        //Debug.Log("target camera rotation: " + _targetRotation);
    }

    public void smoothRotate() {
        Vector3 rotateTo = _currentRotation;
        rotateTo.z = Mathf.Lerp(_currentRotation.z, _targetRotation.z, (Time.timeSinceLevelLoad - _rotationChangedTime) * _rotationSpeed);
        //if (rotateTo - _currentRotation != new Vector3(0.0f, 0.0f, 0.0f)) Debug.Log("rotating by: " + (rotateTo - _currentRotation));
        gameObject.transform.Rotate(rotateTo - _currentRotation);
        _currentRotation = rotateTo;
    }
}
