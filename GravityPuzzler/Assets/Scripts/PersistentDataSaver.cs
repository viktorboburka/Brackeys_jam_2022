using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public sealed class PersistentDataSaver {
    static PersistentData _instance;
    public static string DataDirectory {
        get {
            if (Application.platform == RuntimePlatform.Android) return Path.Combine(Application.persistentDataPath);
            return Path.Combine(Directory.GetCurrentDirectory());
        }
    }
    static string DataPath => Path.Combine(DataDirectory, "save-state.json");

    public static PersistentData Instance {
        get {
            if (_instance == null) {
                var fileData = "";
                try {
                    Debug.Log($"Application.persistentDataPath: {Application.persistentDataPath}, Current Directory: {Directory.GetCurrentDirectory()}");
                    using var reader = new StreamReader(DataPath);
                    fileData = reader.ReadToEnd();
                    _instance = JsonConvert.DeserializeObject<PersistentData>(fileData);
                } catch {
                    _instance = new PersistentData();
                }

                if (JsonConvert.SerializeObject(_instance) != fileData) Save();
            }
            return _instance;
        }
    }


    static bool _saving;
    static bool _shouldResave;
    public static void Save()
    {
        if (_saving) _shouldResave = true;
        else _ = SaveInternal();
    }

    static async Task SaveInternal()
    {
        _saving = true;
        await Task.Delay(10);
        _shouldResave = false;

        Debug.Log($"Writing PersistentData to {DataPath}");
        {
            using var writer = new StreamWriter(DataPath);
            await writer.WriteAsync(JsonConvert.SerializeObject(_instance));
        }
        _saving = false;

        if (_shouldResave) Save();
    }

    PersistentDataSaver() { }
}
