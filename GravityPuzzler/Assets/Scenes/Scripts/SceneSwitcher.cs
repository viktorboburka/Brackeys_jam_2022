using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private Player player;
    private int sceneCount;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        sceneCount = SceneManager.sceneCountInBuildSettings;
        Debug.Log("Scene count is: " + sceneCount);
    }

    void Update()
    {
        if (player.isDead() && Input.GetKey("space"))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        if ((player.isDead() || player.survived()) && Input.GetKey("escape"))
        {
            SceneManager.LoadScene(0);
        }
        if (!player.isDead() && player.survived() && Input.GetKey("return"))
        {
            Scene scene = SceneManager.GetActiveScene();
            int index = scene.buildIndex;
            int nextScene = index + 1;
            Debug.Log("Next scene index is: " + nextScene);
            if(nextScene < sceneCount)
            {
                SceneManager.LoadScene(nextScene);
                
            } else
            {
                return;
            }
            
        }
    }
}
