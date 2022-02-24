using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour {



    // Start is called before the first frame update
    void Start()
    {
        Level.activeLevel.onMemorySpawn();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "KillerPlatform") {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false;
            Level.activeLevel.onMemoryKilled();
            //TODO: play death animation
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            Level.activeLevel.onMemorySaved();
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false;
            //TODO: play save animation
            Destroy(gameObject);
        }
    }
}
