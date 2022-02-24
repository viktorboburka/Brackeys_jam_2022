using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {
    private Player _player;
    [SerializeField]
    private GameObject _gameover;
    [SerializeField]
    private GameObject _complete;
    [SerializeField]
    private GameObject _completeFull;
    [SerializeField]
    private GameObject[] _memoryImages;
    [SerializeField]
    private GameObject _help;

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
        _memoriesCollected = level.savedMemoryCount;
        for (int i = 0; i < _memoryImages.Length; i++) {
            if (i < _memoriesCollected) {
                //Debug.Log(_memoryImages[i]);
                _memoryImages[i].SetActive(true);
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
        if (level.state == Level.State.LOST && !_gameover.activeInHierarchy) {
            _gameover.SetActive(true);
        }

        if (level.state == Level.State.WON) {
            if (level.memoryCount == level.savedMemoryCount) {
                if (!_completeFull.activeInHierarchy) _completeFull.SetActive(true);
            } else {
                if (!_complete.activeInHierarchy) _complete.SetActive(true);
            }
        }
    }
}
