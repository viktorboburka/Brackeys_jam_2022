using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventsManager : MonoBehaviour {
    public void PlayButton()
    {
        SceneManager.LoadScene(Level.scenePaths[0]);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
