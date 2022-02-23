using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour {
    public enum GravityDirection {
        DOWN,
        LEFT,
        UP,
        RIGHT,
    }
    [System.Serializable]
    public class GravityChange {
        public GravityDirection direction = GravityDirection.DOWN;
        public float duration = 5;
    }

    [SerializeField]
    private List<GravityChange> _timeline = new List<GravityChange>();

    [SerializeField]
    private float _terminalVelocity = 5.0f;
    [SerializeField]
    private float _gravityScale = 3.0f;

    int timelineIndex = 0;
    float timeSinceLast = 0;


    private Dictionary<GravityDirection, Vector2> _gravityVectors;

    private CameraControl cameraControl;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();//GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        initGravity();
        changeGravity(_timeline[0].direction);
    }

    bool _inputTouched = false;
    // Update is called once per frame
    void Update()
    {
        if (_inputTouched)
        {
            triggerTimedGravityChanges();
        } else
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (System.Math.Abs(horizontalInput) > 0.05f) _inputTouched = true;
        }
    }

    void triggerTimedGravityChanges()
    {
        timeSinceLast += Time.deltaTime;
        if (_timeline[timelineIndex].duration < timeSinceLast)
        {
            timeSinceLast -= _timeline[timelineIndex].duration;
            timelineIndex = (timelineIndex + 1) % _timeline.Count;
            changeGravity(_timeline[timelineIndex].direction);
        }
    }


    private void changeGravity(GravityDirection changeTo)
    {
        if (_gravityVectors.TryGetValue(changeTo, out var newGravity))
        {
            Physics2D.gravity = newGravity * _gravityScale;
            cameraControl.setTargetRotation(changeTo);
            player.changeDirection(_timeline[timelineIndex].direction);
            player.rotateControls(changeTo);
        }
    }

    void initGravity()
    {
        Physics2D.gravity *= _gravityScale;

        _gravityVectors = new Dictionary<GravityDirection, Vector2>();
        _gravityVectors.Add(GravityDirection.UP, new Vector2(0.0f, 9.8f));
        _gravityVectors.Add(GravityDirection.DOWN, new Vector2(0.0f, -9.8f));
        _gravityVectors.Add(GravityDirection.LEFT, new Vector2(-9.8f, 0.0f));
        _gravityVectors.Add(GravityDirection.RIGHT, new Vector2(9.8f, 0.0f));

        GameObject[] allRigidBodyObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in allRigidBodyObjects)
        {
            //Debug.Log(go.ToString());
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            rb.drag = GetDragFromAcceleration(Physics.gravity.magnitude, _terminalVelocity);
        }
    }

    public static float GetDrag(float aVelocityChange, float aFinalVelocity)
    {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
    {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }
}
