using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityInfluenced
{
    private float _acceleration = 130;
    private float _accelerationAir = 20;
    private string _gravityDir;
    private bool _isDead = false;
    private bool _survived = false;

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
        if (!_isDead)
        {

            var hit = Physics2D.Raycast(transform.position, Physics2D.gravity, 1.02f, ~(1 << 2));
            Vector2 velVec;
            switch (_gravityDir)
            {
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
            if (hit)
            {
                GetComponent<Rigidbody2D>().velocity += velVec * _acceleration * Time.deltaTime;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity += velVec * _accelerationAir * Time.deltaTime;
            }
        } else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        
    }

    public void rotateControls(string gravity) {
        _gravityDir = gravity;
    }
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "KillerPlatform")
        {
            _isDead = true;
            Debug.Log("you are dead");
        }
    }


    public bool isDead()
    {
        return _isDead;
    }

    public void setSurvived(bool survived)
    {
        _survived = survived;
    }

    public bool survived()
    {
        return _survived;
    }
}

