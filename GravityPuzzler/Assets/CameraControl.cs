using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            rotate90();
        }
    }

    public void rotate90() {
        //transform.rotation
        transform.Rotate(0, 0, -90);
    }
}
