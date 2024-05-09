using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using tomi.SaveSystem;

[System.Serializable]
public class SaveMetaData
{
    public string saveName;
    public string description;
    public string thumbnailPath;  // Path to the screenshot thumbnail, if any

    public SaveMetaData(string name, string desc)
    {
        saveName = name;
        description = desc;
        thumbnailPath = "";
    }
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public List<string> saveFiles = new List<string>(); // List to hold the names and descriptions of all save files

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
        if (saveName == null)
        {
            saveName = GenerateSaveName(isAutoSave);
        }

        string metaDataPath = GetFilePath(saveName + "_meta");
        string dataPath = GetFilePath(saveName);

        await Task.Run(() =>
        {
            SaveData(saveName, saveData, metaDataPath, dataPath, isAutoSave);
        });

        StartCoroutine(CaptureThumbnail(saveName));
        RefreshSaveFilesList();
        Debug.Log("Saved: " + metaDataPath);
    }


    private void SaveData(string saveName, SaveData saveData, string metaDataPath, string dataPath, bool isAutoSave)
    {
        SaveMetaData metaData = new SaveMetaData(saveName, GenerateSaveDescription(isAutoSave));
        string jsonMeta = JsonUtility.ToJson(metaData);
        File.WriteAllText(metaDataPath, jsonMeta);

        if (saveData != null)
        {
            string jsonData = JsonUtility.ToJson(saveData, true); 
            File.WriteAllText(dataPath, jsonData);
        }
    }

    public void Load(string saveName)
    {
        string path = GetFilePath(saveName);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            Debug.Log("Data loaded successfully.");
        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }

    public void DeleteSave(string saveName)
    {
        string basePath = Application.persistentDataPath + "/saves/" + saveName;
        string[] fileExtensions = { ".json", "_meta.json", "_thumbnail.png" };

        foreach (var extension in fileExtensions)
        {
            string filePath = basePath + extension;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Deleted file: " + filePath);
            }
        }

        RefreshSaveFilesList();
    }


    private string GenerateSaveName(bool isAutoSave)
    {
        return (isAutoSave ? "autosave_" : "save_") + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
    }

    private string GenerateSaveDescription(bool isAutoSave)
    {
        string time = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        return isAutoSave ? "Autosave | " + time : "Manual Save | " + time;
    }

    private IEnumerator CaptureThumbnail(string saveName)
    {
        yield return new WaitForEndOfFrame(); // Wait for the frame to be rendered
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] bytes = screenshot.EncodeToPNG();
        string filePath = Application.persistentDataPath + "/saves/" + saveName + "_thumbnail.png";
        File.WriteAllBytes(filePath, bytes);
        Destroy(screenshot); // Free the texture memory
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
        var files = Directory.GetFiles(path, "*_meta.json");
        foreach (var file in files)
        {
            string json = File.ReadAllText(file);
            SaveMetaData metaData = JsonUtility.FromJson<SaveMetaData>(json);
            saveFiles.Add(metaData.saveName + " | " + metaData.description);
        }
    }
}
