using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {
    static MusicPlayer instance;
    private Animator playerAnimator;
    private AudioSource source;

    void OnEnable()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            source = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnDisable()
    {
        if (instance == this) instance = null;
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene arg0, LoadSceneMode arg1)
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player")?.transform.GetComponentInChildren<Animator>();
        if (playerAnimator) playerAnimator.enabled = false;
    }

    public void StartPlayerAnimator()
    {
        if (playerAnimator && !playerAnimator.enabled) playerAnimator.enabled = true;
        if (!source.isPlaying) source.Play();
    }
}
