using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour {

    static Material colored;
    float _deathTime = float.PositiveInfinity;

    [SerializeField]
    Color _color = Color.green;


    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    public static Sprite LoadSprite(string name)
    {
        Sprite sprite;
        if (!sprites.TryGetValue(name, out sprite)) {
            sprite = Resources.Load<Sprite>(name);
            sprites.Add(name, sprite);
        }
        return sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        Level.activeLevel.onMemorySpawn();
        colored ??= Resources.Load<Material>("Colored");
        colored.SetColor("_Color_Outside", Color.white);
        colored.SetColor("_Color", Color.white);
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - _deathTime > 0) {
            if (Time.timeSinceLevelLoad - _deathTime > 0.5f) {
                colored.SetFloat("_Animate", 10f);
                colored.SetColor("_Color_Outside", colored.GetColor("_Color"));
                Destroy(gameObject);
            } else {
                colored.SetFloat("_Animate", (Time.timeSinceLevelLoad - _deathTime) * 10f);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "KillerPlatform") {
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false;
            Level.activeLevel.onMemoryKilled();

            //TODO: play death animation
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            Level.activeLevel.onMemorySaved(_color, GetComponent<SpriteRenderer>().sprite);
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.enabled = false;

            colored.SetFloat("_Animate", 0f);
            colored.SetColor("_Color_Outside", colored.GetColor("_Color"));
            colored.SetColor("_Color", _color);

            _deathTime = Time.timeSinceLevelLoad;
            Destroy(GetComponentInChildren<SpriteRenderer>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject);
        }
    }
}
