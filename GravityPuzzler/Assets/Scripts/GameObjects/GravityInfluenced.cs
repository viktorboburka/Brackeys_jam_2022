using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInfluenced : MonoBehaviour
{

    private int _reachableWidth = 50;
    private int _reachableHeight = 50;

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
        if (gameObject.transform.position.y < -_reachableHeight) {
            Vector2 newPos = new Vector2(gameObject.transform.position.x, _reachableHeight);
            gameObject.transform.position = newPos;
        }
        if (gameObject.transform.position.y > _reachableHeight) {
            Vector2 newPos = new Vector2(gameObject.transform.position.x, -_reachableHeight);
            gameObject.transform.position = newPos;
        }

        if (gameObject.transform.position.x < -_reachableWidth) {
            Vector2 newPos = new Vector2(_reachableWidth, gameObject.transform.position.y);
            gameObject.transform.position = newPos;
        }
        if (gameObject.transform.position.x > _reachableWidth) {
            Vector2 newPos = new Vector2(-_reachableWidth, gameObject.transform.position.y);
            gameObject.transform.position = newPos;
        }
    }
}
