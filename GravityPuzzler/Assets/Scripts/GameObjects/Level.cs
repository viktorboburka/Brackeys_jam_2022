using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Level : MonoBehaviour {

    public static readonly List<string> scenePaths = new List<string>{
        "Assets/Scenes/Level1.unity",
        "Assets/Scenes/Level1.5.unity",
        "Assets/Scenes/Level2.unity",
        "Assets/Scenes/Level3.unity",
        "Assets/Scenes/Level4.unity",
        "Assets/Scenes/Level5.unity",
    };


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

    [SerializeField]
    AudioClip _memoryKilled;
    [SerializeField]
    AudioClip _memorySaved;

    #region memory-related bookkeeping
    public class SavedMemory {
        public Color color;
        public Sprite sprite;

        public SavedMemory(Color color, Sprite sprite)
        {
            this.color = color;
            this.sprite = sprite;
        }
    };
    public readonly List<SavedMemory> savedMemories = new List<SavedMemory>();
    public void onMemorySaved(Color color, Sprite sprite)
    {
        // do not change stats after game over
        if (state > State.RUNNING) return;
        GetComponent<AudioSource>().PlayOneShot(_memorySaved);
        savedMemories.Add(new SavedMemory(color, sprite));
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
        GetComponent<AudioSource>().PlayOneShot(_memoryKilled);
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
            if (savedMemories.Count > 0) {
                state = State.WON;
                PersistentData.Update(data => {
                    var scene = SceneManager.GetActiveScene();
                    Debug.Log($"scene: {scene.path}");
                    if (data.highScores == null) data.highScores = new Dictionary<string, PersistentData.Score[]>();
                    var score = savedMemories.Select(v => new PersistentData.Score(v.color, v.sprite.name)).ToArray();
                    Debug.Log($"{score.Length}");

                    if (data.highScores.TryGetValue(scene.path, out var prevValue)) {
                        Debug.Log($"{prevValue.Length} <= {score.Length}");
                        if (prevValue.Length <= score.Length) {
                            data.highScores.Remove(scene.path);
                            data.highScores.Add(scene.path, score);
                        }
                    } else {
                        data.highScores.Add(scene.path, score);
                    }
                    Debug.Log($"{data.highScores != null}");
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

#if UNITY_EDITOR
        // this cleanup is not strictly necessary but prevents trashing git history
        var colored = Resources.Load<Material>("Colored");
        colored.SetColor("_Color_Outside", Color.white);
        colored.SetColor("_Color", Color.white);
        colored.SetFloat("_Animate", 0f);
        colored.SetVector("_Player_Pos", Vector3.zero);
#endif
    }
    #endregion
}
