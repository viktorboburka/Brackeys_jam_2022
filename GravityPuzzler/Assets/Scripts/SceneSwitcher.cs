using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    private Player player;
    private int sceneCount;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        sceneCount = SceneManager.sceneCountInBuildSettings;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        /*if (player.getTimeOfDeath() > Time.timeSinceLevelLoad + 3f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/
        if (player.isDead() && Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
        if (!player.isDead() && player.survived() && Input.GetKey(KeyCode.Return)) {
            Scene scene = SceneManager.GetActiveScene();
            int index = scene.buildIndex;
            int nextScene = index + 1;
            if (nextScene >= sceneCount) {
                return;
            } else {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
