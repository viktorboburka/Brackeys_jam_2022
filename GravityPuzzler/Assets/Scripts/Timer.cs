using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    [SerializeField]
    private Text timerText;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                player.setSurvived(true);
                PersistentData.Update(data => {
                    var scene = SceneManager.GetActiveScene().buildIndex;
                    data.lastLevel = System.Math.Max(scene + 1, data.lastLevel);
                });
            }
            if (timeRemaining < 0) {
                timeRemaining = 0;
            }
        }
        //Debug.Log(timeRemaining);
        DisplayTime(timeRemaining);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = "Time left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float getTime()
    {
        return timeRemaining;
    }
}