using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButtons : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        var lastLevel = PersistentData.Instance.lastLevel;
        var highScores = PersistentData.Instance.highScores;
        var prefab = Resources.Load<GameObject>("Load Level");

        for (var i = 0; i < lastLevel; ++i) {
            var obj = Instantiate(prefab, transform);
            var text = obj.transform.GetComponentInChildren<Text>();
            text.text = $"Night {i + 1}";
            var loader = obj.GetComponent<LoadScene>();
            loader.scene = i + 1;
            if (highScores != null) {
                var scene = SceneUtility.GetScenePathByBuildIndex(i + 1);
                Debug.Log($"Scene: \"{scene}\" i: {i + 1}");
                if (highScores.TryGetValue(scene, out var highscore))
                    loader.UpdateMemories(highscore);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
