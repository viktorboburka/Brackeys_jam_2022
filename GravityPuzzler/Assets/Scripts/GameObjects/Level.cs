using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {
    public enum State {
        INIT = 0, // before first input
        PAUSED = 1,
        RUNNING = 2,
        LOST = 3,
        WON = 4,
    }
    public State state {
        get;
        private set;
    }

    #region memory-related bookkeeping
    public int savedMemoryCount {
        get;
        private set;
    }
    public void onMemorySaved()
    {
        // do not change stats after game over
        if (state > State.RUNNING) return;
        savedMemoryCount++;
        reduceRemainingMemories();
    }

    public int memoriesLeft {
        get;
        private set;
    }
    public int memoryCount {
        get;
        private set;
    }
    public void onMemoryKilled()
    {
        // do not change stats after game over
        if (state > State.RUNNING) return;
        reduceRemainingMemories();
    }
    private void reduceRemainingMemories()
    {
        if (memoriesLeft == 0) {
            Debug.LogWarning("Memories left can't be smaller than 0");
            return;
        }
        memoriesLeft--;

        if (memoriesLeft == 0) {
            if (savedMemoryCount > 0) {
                state = State.WON;
                Time.timeScale = 0;
                PersistentData.Update(data => {
                    var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                    Debug.Log($"scene: {scene.path} ({scene.buildIndex})");
                    data.lastLevel = System.Math.Max(scene.buildIndex + 1, data.lastLevel);
                    if (data.highScores == null) data.highScores = new Dictionary<string, int>();
                    if (data.highScores.TryGetValue(scene.path, out var prevValue)) {
                        if (prevValue < savedMemoryCount) {
                            data.highScores.Remove(scene.path);
                            data.highScores.Add(scene.path, savedMemoryCount);
                        }
                    } else {
                        data.highScores.Add(scene.path, savedMemoryCount);
                    }
                });
            } else {
                state = State.LOST;
            }
        }
    }
    public void onMemorySpawn()
    {
        // do not change stats after game over
        if (state > State.RUNNING) return;
        memoriesLeft++;
        memoryCount++;
    }
    #endregion

    public void killPlayer()
    {
        if (state == State.RUNNING) {
            state = State.LOST;
        }
    }

    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (state == State.RUNNING) {
                state = State.PAUSED;
                Time.timeScale = 0;
            } else if (state == State.PAUSED) {
                state = State.RUNNING;
                Time.timeScale = 1;
            }
        }
        if (state == State.PAUSED && Input.GetKeyUp(KeyCode.M)) {
            SceneManager.LoadScene(0);
        }
    }

    public void OnMovement()
    {
        if (state == State.INIT) {
            state = State.RUNNING;
            Time.timeScale = 1;
        }
    }

    #region fast, static accessor
    static Level _activeLevel = null;
    public static Level activeLevel => _activeLevel;
    void OnEnable()
    {
        if (_activeLevel) Debug.LogWarning("There can't be two active levels");
        _activeLevel = this;
    }
    void OnDisable()
    {
        _activeLevel = null;
    }
    #endregion
}
