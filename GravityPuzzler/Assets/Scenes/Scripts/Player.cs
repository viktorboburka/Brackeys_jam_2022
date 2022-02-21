using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityInfluenced
{
    private float _acceleration = 130;
    private float _accelerationAir = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        calculateMovement();
    }

    void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        var hit = Physics2D.Raycast(transform.position, Physics2D.gravity, 1.02f, ~(1 << 2));
        if (hit) {
            GetComponent<Rigidbody2D>().velocity += new Vector2(horizontalInput, 0) * _acceleration * Time.deltaTime;
        } else {
            GetComponent<Rigidbody2D>().velocity += new Vector2(horizontalInput, 0) * _accelerationAir * Time.deltaTime;
        }
    }
}
