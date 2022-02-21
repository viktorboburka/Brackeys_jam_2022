using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public float _terminalVelocity = 10.0f;
    public float _gravityScale = 3.0f;

    private Dictionary<string, Vector2> _gravityVectors;
    private string _currentGravity = "down";


    // Start is called before the first frame update
    void Start()
    {
        initGravity();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            changeGravity();
            Debug.Log(Physics2D.gravity);
            Debug.Log(_currentGravity);
        }
    }

    void changeGravity() {
        string gravStr = "down";
        switch (_currentGravity) {
            case "down":
                gravStr = "left";
                break;
            case "up":
                gravStr = "right";
                break;
            case "left":
                gravStr = "up";
                break;
            case "right":
                gravStr = "down";
                break;
        }

        Vector2 newGravity;
        bool ret = _gravityVectors.TryGetValue(gravStr, out newGravity);
        if (ret) {
            Physics2D.gravity = newGravity * _gravityScale;
            _currentGravity = gravStr;
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
            Debug.Log(go.ToString());
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
