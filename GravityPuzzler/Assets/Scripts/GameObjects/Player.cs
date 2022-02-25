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
    private Animator animator;
    private float floorDistance;


    [SerializeField]
    private PhysicsMaterial2D _deadMaterial;
    [SerializeField]
    private GameObject _colorSource;
    private Vector3 leftOffset;
    static Material colored;

    void Start()
    {
        colored ??= Resources.Load<Material>("Colored");

        animator = GetComponentInChildren<Animator>();
        var boxCollider = GetComponent<BoxCollider2D>();
        floorDistance = boxCollider.size.y / 2 - boxCollider.offset.y + 0.1f;
        leftOffset = new Vector3(-(boxCollider.size.x / 2 + boxCollider.offset.x), 0, 0);
    }
    bool movementAllowed => Level.activeLevel.state == Level.State.INIT || Level.activeLevel.state == Level.State.RUNNING;
    bool isDead => Level.activeLevel.state == Level.State.LOST;

    protected override void Update()
    {
        base.Update();

        if (movementAllowed) {
            calculateMovement();
        }
        colored.SetVector("_Player_Pos", _colorSource.transform.position);
    }

    bool isOnGround()
    {

        var hitCenter = Physics2D.Raycast(transform.position, Physics2D.gravity, floorDistance, ~(1 << 2));
        var hitLeft = Physics2D.Raycast(transform.position + leftOffset, Physics2D.gravity, floorDistance, ~(1 << 2));
        var hitRight = Physics2D.Raycast(transform.position - leftOffset, Physics2D.gravity, floorDistance, ~(1 << 2));

        return hitCenter || hitLeft || hitRight;
    }

    void calculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool hasInput = System.Math.Abs(horizontalInput) > 0.05f;
        if (hasInput) {
            Level.activeLevel.OnMovement();
        }

        var hit = isOnGround();
        //Debug.Log(horizontalInput);

        animator.SetBool("walking", hit && hasInput);

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
        if (isDead) {
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
            Level.activeLevel.killPlayer();
            GetComponent<Rigidbody2D>().sharedMaterial = _deadMaterial;
            GetComponent<Rigidbody2D>().freezeRotation = false;

            var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites) {
                var rb = sprite.gameObject.AddComponent<Rigidbody2D>();
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.drag = 1f;
                rb.angularDrag = 1f;
                sprite.gameObject.AddComponent<PolygonCollider2D>();
                sprite.gameObject.AddComponent<GravityInfluenced>();
                var skin = sprite.gameObject.GetComponent<UnityEngine.U2D.Animation.SpriteSkin>();
                if (skin != null) Destroy(skin);
                rb.AddForce(Vector2.up.Rotate(Random.Range(0f, 360f)) * 1000f);
                const float torque = 100f;
                rb.AddTorque(Random.Range(-torque, torque));
            }
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(this);
            Destroy(GetComponentInChildren<Animator>());

            //Debug.Log("you are dead");
        }
    }
}

