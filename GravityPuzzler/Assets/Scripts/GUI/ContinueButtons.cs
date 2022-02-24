using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtons : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        var lastLevel = PersistentData.Instance.lastLevel;
        var prefab = Resources.Load<GameObject>("Load Level");

        for (var i = 0; i < lastLevel; ++i) {
            var obj = Instantiate(prefab, transform);
            var text = obj.transform.GetComponentInChildren<Text>();
            text.text = $"Level {i + 1}";
            var loader = obj.GetComponent<LoadScene>();
            loader.scene = i + 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
