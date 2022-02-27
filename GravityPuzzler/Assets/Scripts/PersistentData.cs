using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

/**
 * This class contains all the save state.
 *
 * Reading: PersistentData.Instance.field
 *
 * Writing:
 * PersistentData.Update((data) => {
 *   data.field = value;
 * });
 *
 * Adding new fields: just add a public member at the start.
 */
public sealed class PersistentData {
    public class PersistableColor {
        public float r, g, b, a;
        public Color ToColor() { return new Color(r, g, b, a); }
    };
    public class Score {
        public PersistableColor color;
        public string sprite;

        public Score(Color color, string sprite)
        {
            this.color = new PersistableColor { r = color.r, g = color.g, b = color.b, a = color.a };
            this.sprite = sprite;
        }
    };
    public Dictionary<string, Score[]> highScores = new Dictionary<string, Score[]>();
    public bool helpShown;

    public delegate void Updater(PersistentData instance);
    public static void Update(Updater updater)
    {
        updater(PersistentDataSaver.Instance);
        PersistentDataSaver.Save();
    }

    public static PersistentData Instance { get { return PersistentDataSaver.Instance; } }
}
