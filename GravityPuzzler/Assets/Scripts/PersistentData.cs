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
    public int lastLevel;

    public delegate void Updater(PersistentData instance);
    public static void Update(Updater updater)
    {
        updater(PersistentDataSaver.Instance);
        PersistentDataSaver.Save();
    }

    public static PersistentData Instance { get { return PersistentDataSaver.Instance; } }
}
