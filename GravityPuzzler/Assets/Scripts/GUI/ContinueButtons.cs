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
        const int gap = 40;
        const int width = 100;
        var x = -(width + gap) * (lastLevel - 1) / 2;
        for (var i = 0; i < lastLevel; ++i) {
            var obj = Instantiate(prefab, transform);
            obj.transform.localPosition = new Vector3(x, 0, 0);
            x += width + gap;
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
