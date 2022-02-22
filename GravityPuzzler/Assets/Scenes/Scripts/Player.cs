using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityInfluenced
{
    private float _acceleration = 130;
    private float _accelerationAir = 20;
    private string _gravityDir;

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
        Vector2 velVec;
        switch(_gravityDir) {
            case "down":
                velVec = new Vector2(horizontalInput, 0);
                break;
            case "up":
                velVec = new Vector2(-horizontalInput, 0);
                break;
            case "left":
                velVec = new Vector2(0, -horizontalInput);
                break;
            case "right":
                velVec = new Vector2(0, horizontalInput);
                break;
            default:

                velVec = new Vector2(horizontalInput, 0);
                break;
        }
        if (hit) {
            GetComponent<Rigidbody2D>().velocity += velVec * _acceleration * Time.deltaTime;
        }
        else {
            GetComponent<Rigidbody2D>().velocity += velVec * _accelerationAir * Time.deltaTime;
        }
    }

    public void rotateControls(string gravity) {
        _gravityDir = gravity;
    }
}
