using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public float terminalVelocity = 10.0f;
    public float gravityScale = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity *= gravityScale;
        GameObject[] allRigidBodyObjects = GameObject.FindObjectsOfType(typeof (GameObject)) as GameObject[];
        foreach( GameObject go in allRigidBodyObjects) {
            Debug.Log(go.ToString());
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            rb.drag = GetDragFromAcceleration(Physics.gravity.magnitude, terminalVelocity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("switch gravity");
            Physics2D.gravity = -Physics2D.gravity;
        }
    }


    public static float GetDrag(float aVelocityChange, float aFinalVelocity) {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity) {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }
}
