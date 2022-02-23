using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;
    [SerializeField]
    private float _rotationSpeed = 2f;
    private float _rotationChangedTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        _currentRotation = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        _targetRotation = _currentRotation;
    }

    // Update is called once per frame
    void Update()
    {
        smoothRotate();
    }

    public void setTargetRotation(string target) {
        switch (target) {
            case "up":
                _targetRotation = new Vector3 (0.0f, 0.0f, 180.0f);
                break;
            case "down":
                _targetRotation = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case "left":
                _targetRotation = new Vector3(0.0f, 0.0f, -90f);
                break;
            case "right":
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
        rotateTo.z = Mathf.Lerp(_currentRotation.z, _targetRotation.z, (Time.time - _rotationChangedTime) * _rotationSpeed);
        //if (rotateTo - _currentRotation != new Vector3(0.0f, 0.0f, 0.0f)) Debug.Log("rotating by: " + (rotateTo - _currentRotation));
        gameObject.transform.Rotate(rotateTo - _currentRotation);
        _currentRotation = rotateTo;
    }
}
