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
    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

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
            Sprite sprite;
            if (!sprites.TryGetValue(savedMemory.sprite, out sprite)) {
                sprite = Resources.Load<Sprite>(savedMemory.sprite);
                sprites.Add(savedMemory.sprite, sprite);
            }
            Debug.Log(savedMemory.sprite);
            _memories[i].GetComponent<Image>().sprite = sprite;
            ++i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
