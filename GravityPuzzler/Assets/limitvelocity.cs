using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitvelocity : MonoBehaviour
{
    public float terminalVelocity;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.gravity.y > 0) rb.drag = GetDragFromAcceleration(Physics.gravity.magnitude, terminalVelocity);
    }

    public static float GetDrag(float aVelocityChange, float aFinalVelocity) {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity) {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }

}
