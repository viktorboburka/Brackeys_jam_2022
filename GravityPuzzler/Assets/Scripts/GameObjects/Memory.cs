using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{

    private float _timeOfDeath = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > _timeOfDeath /* + animation duration*/) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "KillerPlatform") {
            _timeOfDeath = Time.timeSinceLevelLoad;
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false;
            //TODO: play death animation
        }
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            player.savedMemory();
            _timeOfDeath = Time.timeSinceLevelLoad;
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false;
            //TODO: play save animation
        }
    }

}
