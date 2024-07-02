using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tomi.SaveSystem;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public List<string> saveFiles = new List<string>();
    public Sprite[] levelImages;
    private const string SaveFileName = "GameData.json";
    //private const int MaxAutosaves = 4;

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

            //vsync
            QualitySettings.vSyncCount = 1;
            saveData.playerProfile.vsync = 1;

            SaveAsync(saveData);
        }
        else
        {
            /*----IMPORTANT--REMOVE WHEN GAME DONE-----*/
            //SaveData.Current = Load();
            SaveData saveData = new SaveData();

            //vsync
            QualitySettings.vSyncCount = 1;
            saveData.playerProfile.vsync = 1;

            SaveAsync(saveData);
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

}
