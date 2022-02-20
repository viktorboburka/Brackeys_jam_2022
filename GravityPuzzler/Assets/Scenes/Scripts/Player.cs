using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityInfluenced
{
    private int _speed = 130;
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
        GetComponent<Rigidbody2D>().velocity += new Vector2(horizontalInput, 0) * _speed * Time.deltaTime;
        
    }
}
