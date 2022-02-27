using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventsManager : MonoBehaviour {
    void Start()
    {
        if (PersistentData.Instance.highScores.Count < 1) {
            SceneManager.LoadScene(Level.scenePaths[0]);
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
