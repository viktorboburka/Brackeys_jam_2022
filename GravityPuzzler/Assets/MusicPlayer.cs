using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {
    private Animator playerAnimator;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
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
