using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButtons : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        var highScores = PersistentData.Instance.highScores;
        if (highScores == null) return;
        var prefab = Resources.Load<GameObject>("Load Level");

        int i = 0;
        foreach (var path in Level.scenePaths) {
            ++i;
            var obj = Instantiate(prefab, transform);
            var text = obj.transform.GetComponentInChildren<Text>();
            text.text = $"Night {i}";
            var loader = obj.GetComponent<LoadScene>();
            loader.scenePath = path;
            if (highScores.TryGetValue(path, out var highscore))
                loader.UpdateMemories(highscore);
            else
                return;
        }
    }

}
