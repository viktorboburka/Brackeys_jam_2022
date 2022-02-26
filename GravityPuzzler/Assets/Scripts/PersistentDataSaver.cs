using System;
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
                    _instance = JsonConvert.DeserializeObject<PersistentData>(fileData) ?? new PersistentData();
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
        Debug.Log($"Save() _shouldResave: {_shouldResave} _saving: {_saving}");
        if (_saving) _shouldResave = true;
        else _ = SaveInternal();
    }

    static async Task SaveInternal()
    {
        Debug.Log("SaveInternal()");
        _saving = true;
        _shouldResave = false;
        try {
            var value = JsonConvert.SerializeObject(_instance);
            Debug.Log($"Writing PersistentData to {DataPath}");
            Debug.Log(value);
            {
                using var writer = new StreamWriter(DataPath);
                await writer.WriteAsync(value);
            }
            await Task.Delay(10);
        } catch (Exception e) {
            Debug.LogError(e);
        }
        _saving = false;
        Debug.Log($"_saving = false");

        if (_shouldResave) Save();
    }

    PersistentDataSaver() { }
}
