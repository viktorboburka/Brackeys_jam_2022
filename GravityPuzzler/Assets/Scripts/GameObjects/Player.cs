using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityInfluenced {

    private GravityControl.GravityDirection _gravityDir;

    private float _acceleration = 150;
    private float _accelerationAir = 150;
    private float _maxSpeed = 10;
    private float _deccelerationMultiplier = 2f;
    private float _deccelerationIdleMultiplier = 0.1f;

    private bool _isDead = false;
    private float _timeOfDeath = Mathf.Infinity;
    private int _savedMemories = 0;
    //[SerializeField]
    private int _numberOfMemoriesLeft = 3;

    [SerializeField]
    private PhysicsMaterial2D _deadMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!_isDead) {
            calculateMovement();
        }
        if (_timeOfDeath == Mathf.Infinity && isDead()) {
            _timeOfDeath = Time.timeSinceLevelLoad;
        }
    }

    void calculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (System.Math.Abs(horizontalInput) > 0.05f) {
            Time.timeScale = 1;
        }

        var hit = Physics2D.Raycast(transform.position, Physics2D.gravity, 1.1f, ~(1 << 2));
        //Debug.Log(horizontalInput);


        Vector2 velVec;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        switch (_gravityDir) {
            case GravityControl.GravityDirection.DOWN:
                //slow down when there is no input
                if (horizontalInput == 0f) {
                    velVec = new Vector2(-rb.velocity.x * _deccelerationIdleMultiplier, 0f);
                    break;
                }

                //accelerate if not at max speed, decceleration is faster
                if (Mathf.Abs(rb.velocity.x) < _maxSpeed) {
                    velVec = new Vector2(horizontalInput, 0);
                    if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(velVec.x)) {
                        velVec *= _deccelerationMultiplier;
                    }
                } else {
                    velVec = new Vector2(0, 0);
                }
                break;
            case GravityControl.GravityDirection.UP:

                //slow down when there is no input
                if (horizontalInput == 0f) {
                    velVec = new Vector2(-rb.velocity.x * _deccelerationIdleMultiplier, 0f);
                    break;
                }

                //accelerate if not at max speed, decceleration is faster
                if (Mathf.Abs(rb.velocity.x) < _maxSpeed) {
                    velVec = new Vector2(-horizontalInput, 0);
                    if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(velVec.x)) {
                        velVec *= _deccelerationMultiplier;
                    }
                } else {
                    velVec = new Vector2(0, 0);
                }
                break;
            case GravityControl.GravityDirection.LEFT:

                //slow down when there is no input
                if (horizontalInput == 0f) {
                    velVec = new Vector2(0f, -rb.velocity.y * _deccelerationIdleMultiplier);
                    break;
                }

                //accelerate if not at max speed, decceleration is faster
                if (Mathf.Abs(rb.velocity.y) < _maxSpeed) {
                    velVec = new Vector2(0, -horizontalInput);
                    if (Mathf.Sign(rb.velocity.y) != Mathf.Sign(velVec.y)) {
                        velVec *= _deccelerationMultiplier;
                    }
                } else {
                    velVec = new Vector2(0, 0);
                }
                break;
            case GravityControl.GravityDirection.RIGHT:
                //slow down when there is no input
                if (horizontalInput == 0f) {
                    velVec = new Vector2(0f, -rb.velocity.y * _deccelerationIdleMultiplier);
                    break;
                }

                //accelerate if not at max speed, decceleration is faster
                if (Mathf.Abs(rb.velocity.y) < _maxSpeed) {
                    velVec = new Vector2(0, horizontalInput);
                    if (Mathf.Sign(rb.velocity.y) != Mathf.Sign(velVec.y)) {
                        velVec *= _deccelerationMultiplier;
                    }
                } else {
                    velVec = new Vector2(0, 0);
                }

                break;
            default:
                velVec = new Vector2(horizontalInput, 0);
                break;
        }

        if (hit) {
            rb.velocity += velVec * _acceleration * Time.deltaTime;
        } else {
            rb.velocity += velVec * _accelerationAir * Time.deltaTime;
        }
    }

    public void changeDirection(GravityControl.GravityDirection targetDir)
    {
        if (_isDead) {
            return;
        }
        float[,] anglesBetweenDir = new float[4, 4] {
            {0, -90, 180, 90},
            {90, 0, -90, 180},
            {180, 90, 0, -90},
            {-90, 180, 90, 0}
        };

        float rotation = anglesBetweenDir[(int)_gravityDir, (int)targetDir];
        gameObject.transform.Rotate(new Vector3(0, 0, rotation));
    }

    public void rotateControls(GravityControl.GravityDirection gravity)
    {
        _gravityDir = gravity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "KillerPlatform") {
            _isDead = true;
            gameObject.GetComponent<Rigidbody2D>().sharedMaterial = _deadMaterial;
            gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;

            //Debug.Log("you are dead");
        }
    }


    public float getTimeOfDeath()
    {
        return _timeOfDeath;
    }

    public bool isDead()
    {
        return _isDead;
    }

    public void savedMemory()
    {
        _savedMemories++;
    }

    public void reduceMemory()
    {
        _numberOfMemoriesLeft--;
    }

    public int getSavedMemoryCount()
    {
        return _savedMemories;
    }

    public int getMemoryLeftCount()
    {
        return _numberOfMemoriesLeft;
    }

}

