using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("switch gravity");
            Physics2D.gravity = -Physics2D.gravity;
        }
    }
}
