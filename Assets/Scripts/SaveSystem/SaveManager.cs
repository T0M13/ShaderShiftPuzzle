using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tomi.SaveSystem;
using System.Runtime.Serialization.Json;
using System.Text;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public List<string> saveFiles = new List<string>();
    public Sprite[] levelImages;
    private const string SaveFileName = "GameData.json";
    public int currentGameVersion = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CheckAndCreateSaveFile();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckAndCreateSaveFile()
    {
        string path = GetFilePath(SaveFileName);
        if (!File.Exists(path))
        {
            SaveData saveData = new SaveData();
            QualitySettings.vSyncCount = 1;
            saveData.playerProfile.vsync = 1;
            SaveAsync(saveData);
        }
        else
        {
            SaveData.Current = Load();
        }
    }

    public async void SaveAsync(SaveData saveData)
    {
        string dataPath = GetFilePath(SaveFileName);
        saveData.saveMetaData = new SaveMetaData
        {
            saveName = SaveFileName,
            description = GenerateSaveDescription(),
            autoSave = true
        };

        await Task.Run(() =>
        {
            string jsonData = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(dataPath, jsonData);
        });
        Debug.Log("Saved: " + dataPath);
    }

    public SaveData Load()
    {
        string path = GetFilePath(SaveFileName);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(jsonData);
        }
        return null;
    }

    public void DeleteSave()
    {
        string filePath = GetFilePath(SaveFileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Deleted save file: " + filePath);
            CheckAndCreateSaveFile();
        }
    }

    private string GenerateSaveDescription()
    {
        return System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    }

    private string GetFilePath(string fileName)
    {
        string folderPath = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        return Path.Combine(folderPath, fileName);
    }

    public bool EnsureCurrentGameVersion()
    {
        string path = GetFilePath(SaveFileName);
        string jsonData = File.ReadAllText(path);
        Dictionary<string, object> jsonDict = DeserializeToDictionary(jsonData);

        if (jsonDict.ContainsKey("playerGameData"))
        {
            var playerGameData = jsonDict["playerGameData"] as Dictionary<string, object>;
            if (!playerGameData.ContainsKey("currentGameVersion"))
            {
                return false;
            }
        }
        return true;
    }

    private Dictionary<string, object> DeserializeToDictionary(string jsonData)
    {
        var dict = new Dictionary<string, object>();
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
        {
            var ser = new DataContractJsonSerializer(dict.GetType());
            dict = ser.ReadObject(ms) as Dictionary<string, object>;
        }
        return dict;
    }

    private string SerializeDictionaryToJson(Dictionary<string, object> dict)
    {
        using (var ms = new MemoryStream())
        {
            var ser = new DataContractJsonSerializer(dict.GetType());
            ser.WriteObject(ms, dict);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }

}
