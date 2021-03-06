using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    private int sceneCount;
    static int reloadedFrom = -1;
    [SerializeField]
    Animator _sleepingAnimator;

    // Start is called before the first frame update
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        if (SceneManager.GetActiveScene().buildIndex == reloadedFrom) {
            _sleepingAnimator.SetBool("start", true);
            _sleepingAnimator.speed = 1.75f;
        }
        reloadedFrom = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            var activeScene = SceneManager.GetActiveScene();
            reloadedFrom = activeScene.buildIndex;
            SceneManager.LoadScene(activeScene.name);
        }
        /*if (player.getTimeOfDeath() > Time.timeSinceLevelLoad + 3f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/
        var state = Level.activeLevel.state;
        bool isEndScreen = state == Level.State.WON || state == Level.State.LOST;
        if (isEndScreen && Input.GetKey(KeyCode.M)) {
            SceneManager.LoadScene(0);
        }

        if (state == Level.State.WON && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))) {
            Scene scene = SceneManager.GetActiveScene();
            int index = Level.scenePaths.IndexOf(scene.path);
            if (index < 0) {
                Debug.LogError("Can't find current scene in level list");
                return;
            }
            if (index + 1 < Level.scenePaths.Count) {
                SceneManager.LoadScene(Level.scenePaths[index + 1]);
            } else {
                SceneManager.LoadScene(0);
            }
        }
    }
}
