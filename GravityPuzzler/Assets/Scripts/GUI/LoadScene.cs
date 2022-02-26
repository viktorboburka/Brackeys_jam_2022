using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadScene : MonoBehaviour {
    public int scene;
    [SerializeField]
    private List<GameObject> _memories = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene(scene);
    }

    public void UpdateMemories(PersistentData.Score[] savedMemories)
    {
        if (_memories == null) return;
        int i = 0;
        foreach (var savedMemory in savedMemories) {
            _memories[i].GetComponent<Image>().sprite = Memory.LoadSprite(savedMemory.sprite);
            ++i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
