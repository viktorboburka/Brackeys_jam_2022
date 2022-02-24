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
    [SerializeField]
    Sprite sprite;

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

    public void UpdateMemories(int savedMemoryCount)
    {
        if (_memories == null) return;
        for (int i = 0; i < Math.Min(_memories.Count, savedMemoryCount); ++i) {
            _memories[i].GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
