using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityInfluenced
{
    private float _acceleration = 60;
    private float _accelerationAir = 60;
    private string _gravityDir;
    private float _maxSpeed = 15;

    private bool _isDead = false;
    private bool _survived = false;
    private float _timeOfDeath = Mathf.Infinity;

    private int _savedMemories = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        calculateMovement();
        if (_timeOfDeath == Mathf.Infinity && isDead()) {
            _timeOfDeath = Time.timeSinceLevelLoad;
        }
    }

    void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        var hit = Physics2D.Raycast(transform.position, Physics2D.gravity, 1.1f, ~(1 << 2));

        Vector2 velVec;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        switch (_gravityDir)
        {
            case "down":
                if (Mathf.Abs(rb.velocity.x) < _maxSpeed) {
                    velVec = new Vector2(horizontalInput, 0);
                }
                else {
                    velVec = new Vector2(0, 0);
                }
                break;
            case "up":
                if (Mathf.Abs(rb.velocity.x) < _maxSpeed) {
                    velVec = new Vector2(-horizontalInput, 0);
                }
                else {
                    velVec = new Vector2(0, 0);
                }
                break;
            case "left":
                if (Mathf.Abs(rb.velocity.y) < _maxSpeed) {
                    velVec = new Vector2(0, -horizontalInput);
                }
                else {
                    velVec = new Vector2(0, 0);
                }
                break;
            case "right":
                if (Mathf.Abs(rb.velocity.y) < _maxSpeed) {
                    velVec = new Vector2(0, horizontalInput);
                }
                else {
                    velVec = new Vector2(0, 0);
                }
                
                break;
            default:
                velVec = new Vector2(horizontalInput, 0);
                break;
        }
        if (_isDead) {
            velVec = new Vector2(0, 0);
        }
        if (hit) {
            rb.velocity += velVec * _acceleration * Time.deltaTime;
        }
        else {
            rb.velocity += velVec * _accelerationAir * Time.deltaTime;
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


    public float getTimeOfDeath() {
        return _timeOfDeath;
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

    public void savedMemory() {
        _savedMemories++;
    }

}

