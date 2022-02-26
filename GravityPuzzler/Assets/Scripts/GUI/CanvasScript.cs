using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {
    private Player _player;
    [SerializeField]
    private GameObject _gameover;
    [SerializeField]
    private GameObject _gameoverDead;
    [SerializeField]
    private GameObject _complete;
    [SerializeField]
    private GameObject _completeFull;
    [SerializeField]
    private GameObject[] _memoryImages;
    [SerializeField]
    private GameObject _help;
    [SerializeField]
    private GameObject _paused;

    private int _memoriesCollected = 0;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!PersistentData.Instance.helpShown) {
            _help.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var level = Level.activeLevel;
        _memoriesCollected = level.savedMemories.Count;
        for (int i = 0; i < _memoryImages.Length; i++) {
            var image = _memoryImages[i];
            if (i < _memoriesCollected) {
                //Debug.Log(_memoryImages[i]);
                if (!image.activeInHierarchy) image.SetActive(true);
            } else {
                if (image.activeInHierarchy) image.SetActive(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.H)) {
            _help.SetActive(!_help.activeSelf);
            if (!PersistentData.Instance.helpShown) {
                PersistentData.Update(data => {
                    data.helpShown = true;
                });
            }
        }

        if (level.state == Level.State.LOST) {
            if (level.memoriesLeft > 0) {
                if (!_gameoverDead.activeInHierarchy) _gameoverDead.SetActive(true);
            } else {
                if (!_gameover.activeInHierarchy) _gameover.SetActive(true);
            }
        } else {
            if (_gameoverDead.activeInHierarchy) _gameoverDead.SetActive(false);
            if (_gameover.activeInHierarchy) _gameover.SetActive(false);
        }

        if (level.state == Level.State.WON) {
            if (level.memoryCount == level.savedMemories.Count) {
                if (!_completeFull.activeInHierarchy) _completeFull.SetActive(true);
            } else {
                if (!_complete.activeInHierarchy) _complete.SetActive(true);
            }
        } else {
            if (_completeFull.activeInHierarchy) _completeFull.SetActive(false);
            if (_complete.activeInHierarchy) _complete.SetActive(false);
        }

        if (level.state == Level.State.PAUSED) {
            if (!_paused.activeInHierarchy) _paused.SetActive(true);
        } else {
            if (_paused.activeInHierarchy) _paused.SetActive(false);
        }
    }
}
