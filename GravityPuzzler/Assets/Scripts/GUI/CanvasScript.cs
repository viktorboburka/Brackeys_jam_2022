using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private GameObject _gameover;
    [SerializeField]
    private GameObject _complete;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.isDead() && !_player.survived())
        {
            _gameover.SetActive(true);
        }
        if(!_player.isDead() && _player.survived())
        {
            _complete.SetActive(true);
        }
    }
}
