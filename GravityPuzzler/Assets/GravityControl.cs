using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    [SerializeField]
    private float _terminalVelocity = 5.0f;
    [SerializeField]
    private float _gravityScale = 3.0f;
    [SerializeField]
    private List<string> gravChanges;
    private int gravityChangesIdx = 0;
    [SerializeField]
    private List<float> gravChangeTimes;
    private List<bool> gravChangesDone;

    private Dictionary<string, Vector2> _gravityVectors;
    private string _currentGravity = "down";

    private CameraControl cameraControl;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        gravChangesDone = new List<bool>();
        for (int i = 0; i < gravChanges.Count; i++) {
            gravChangesDone.Add(false);
        }

        cameraControl = Camera.main.GetComponent<CameraControl>();//GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        initGravity();
    }

    // Update is called once per frame
    void Update()
    {
        triggerTimedGravityChanges();
    }

    void triggerTimedGravityChanges() {
        for (int i = 0; i < gravChanges.Count; i++) {
            if (gravChangesDone[i]) {
                continue;
            }
            float sumTime = 0.0f;
            for(int j = 0; j <= i; j++) {
                sumTime += gravChangeTimes[j];
            }

            if (Time.time > sumTime) {
                changeGravity(gravChanges[i]);
            }
        }
    }


    public void changeGravity(string changeTo) {
        Vector2 newGravity;
        bool ret = _gravityVectors.TryGetValue(changeTo, out newGravity);
        if (ret) {
            Physics2D.gravity = newGravity * _gravityScale;
            _currentGravity = changeTo;
            cameraControl.setTargetRotation(changeTo);
            player.rotateControls(changeTo);
        }
    }

    public string getCurrentGravity() {
        return _currentGravity;
    }

    void initGravity() {
        Physics2D.gravity *= _gravityScale;

        _gravityVectors = new Dictionary<string, Vector2>();
        _gravityVectors.Add("up", new Vector2(0.0f, 9.8f));
        _gravityVectors.Add("down", new Vector2(0.0f, -9.8f));
        _gravityVectors.Add("left", new Vector2(-9.8f, 0.0f));
        _gravityVectors.Add("right", new Vector2(9.8f, 0.0f));

        GameObject[] allRigidBodyObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in allRigidBodyObjects) {
            //Debug.Log(go.ToString());
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            rb.drag = GetDragFromAcceleration(Physics.gravity.magnitude, _terminalVelocity);
        }
    }

    public static float GetDrag(float aVelocityChange, float aFinalVelocity) {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity) {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }
}
