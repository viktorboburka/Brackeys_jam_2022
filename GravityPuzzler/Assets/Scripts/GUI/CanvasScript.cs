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
    private GameObject[] _memoryImages;
    [SerializeField]
    private GameObject _help;

    private int _memoriesCollected = 0;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H)) {
            _help.SetActive(!_help.activeSelf);
        }
        if (_player.isDead() && !_player.survived()) {
            _gameover.SetActive(true);
        }
        if (!_player.isDead() && _player.survived()) {
            _complete.SetActive(true);
        }
        _memoriesCollected = _player.getSavedMemoryCount();
        for (int i = 0; i < _memoryImages.Length; i++) {
            if (i < _memoriesCollected) {
                _memoryImages[i].SetActive(true);
            }
        }
    }
}
