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
    public List<string> saveFiles = new List<string>(); // List to hold the names and descriptions of all save files
    public Sprite[] levelImages; // Array to hold level images
    private const int MaxAutosaves = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RefreshSaveFilesList();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void SaveAsync(string saveName = null, SaveData saveData = null, bool isAutoSave = false)
    {
        if (isAutoSave)
        {
            ManageAutosaves();  // Check and manage autosave files
        }

        if (saveName == null)
        {
            saveName = GenerateSaveName(isAutoSave);
        }

        string dataPath = GetFilePath(saveName);

        saveData.saveMetaData = new SaveMetaData();

        saveData.saveMetaData.saveName = saveName;
        saveData.saveMetaData.description = GenerateSaveDescription();
        saveData.saveMetaData.autoSave = isAutoSave;

        await Task.Run(() =>
        {
            string jsonData = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(dataPath, jsonData);
        });

        RefreshSaveFilesList();
        Debug.Log("Saved: " + dataPath);
    }

    private void ManageAutosaves()
    {
        var autosaveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves", "autosave_*.json")
            .Select(path => new FileInfo(path))
            .OrderBy(file => file.CreationTime)
            .ToList();

        while (autosaveFiles.Count >= MaxAutosaves)
        {
            if (autosaveFiles.Any())
            {
                File.Delete(autosaveFiles[0].FullName);
                autosaveFiles.RemoveAt(0);
                Debug.Log("Deleted oldest autosave.");
            }
        }
    }

    public SaveData Load(string saveName)
    {
        string path = GetFilePath(saveName);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            Debug.Log("Data loaded successfully.");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found.");
            return null;
        }
    }

    public void DeleteSave(string saveName)
    {
        string filePath = GetFilePath(saveName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Deleted file: " + filePath);
        }
        RefreshSaveFilesList();
    }

    private string GenerateSaveName(bool isAutoSave)
    {
        return (isAutoSave ? "autosave_" : "save_") + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
    }

    private string GenerateSaveDescription()
    {
        return System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"); ;
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/saves/" + fileName + ".json";
    }

    private void RefreshSaveFilesList()
    {
        saveFiles.Clear();
        string path = Application.persistentDataPath + "/saves";
        Directory.CreateDirectory(path); // Ensure directory exists
        var files = Directory.GetFiles(path, "*.json");
        foreach (var file in files)
        {
            string json = File.ReadAllText(file);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
            if (loadedData != null && loadedData.saveMetaData != null)
            {
                saveFiles.Add(loadedData.saveMetaData.saveName);
            }
        }
    }
}
