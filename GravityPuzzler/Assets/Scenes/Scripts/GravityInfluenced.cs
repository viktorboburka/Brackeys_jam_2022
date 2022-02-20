using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInfluenced : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveThroughEdges();
    }

    private void MoveThroughEdges() {
        if (gameObject.transform.position.y < -50) {
            Vector2 newPos = new Vector2(gameObject.transform.position.x, 50);
            gameObject.transform.position = newPos;
        }
        if (gameObject.transform.position.y > 50) {
            Vector2 newPos = new Vector2(gameObject.transform.position.x, -50);
            gameObject.transform.position = newPos;
        }

        if (gameObject.transform.position.x < -90) {
            Vector2 newPos = new Vector2(90, gameObject.transform.position.y);
            gameObject.transform.position = newPos;
        }
        if (gameObject.transform.position.x > 90) {
            Vector2 newPos = new Vector2(-90, gameObject.transform.position.y);
            gameObject.transform.position = newPos;
        }
    }
}
