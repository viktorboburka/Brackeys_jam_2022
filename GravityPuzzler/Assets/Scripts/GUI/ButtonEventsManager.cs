using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventsManager : MonoBehaviour {
    void Start()
    {
        if (PersistentData.Instance.lastLevel < 1) {
            SceneManager.LoadScene(1);
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
