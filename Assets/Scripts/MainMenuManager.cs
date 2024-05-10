using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Events to Execute on Start")]
    public UnityEvent eventsOnStart;

    [Header("Portal")]
    [SerializeField] private float minRandomTime = 20;
    [SerializeField] private float maxRandomTime = 40;
    [SerializeField][ShowOnly] private float timer;
    [SerializeField][ShowOnly] private bool isPortalOpen = false;

    public UnityEvent portalOpen;
    public UnityEvent portalClose;

    [Header("LoadSave Files")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private Transform chapterLoadParent;
    [SerializeField] private GameObject chapterLoadGameObject;
    [SerializeField] private Transform loadedFileParent;
    [SerializeField] private GameObject loadPrefab;
    [SerializeField][ShowOnly] private string tempDeleteFileName = "";
    [SerializeField] private ModalWindowManager deleteSaveFileWindow;

    [SerializeField][ShowOnly] private string tempLoadFileName = "";
    [SerializeField] private ModalWindowManager loadSaveFileWindow;

    public SaveManager SaveManager
    {
        get => saveManager;
        private set => saveManager = value;
    }

    private void Start()
    {

        saveManager = GameObject.FindGameObjectWithTag("SaveManager")?.GetComponent<SaveManager>();
        if (saveManager == null)
        {
            Debug.LogError("MainMenuManager: No saveManager object found! ");
        }
        else
        {
            UpdateChapterLoadButton();
            CreateLoadButtons();
        }

        eventsOnStart?.Invoke();
        SetNewTimer();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TogglePortal();
            SetNewTimer();
        }
    }
    private void UpdateChapterLoadButton()
    {
        if (saveManager.saveFiles.Count > 0)
        {
            // Assuming saveFiles is sorted or you sort it as needed
            string mostRecentSaveFileName = saveManager.saveFiles.Last();

            // Load the most recent save data
            SaveData mostRecentSaveData = saveManager.Load(mostRecentSaveFileName);
            if (mostRecentSaveData != null)
            {
                ChapterLoadSaveButton chapterLoadSaveButton = chapterLoadGameObject.GetComponent<ChapterLoadSaveButton>();

                if (chapterLoadSaveButton != null)
                {
                    //string saveName = mostRecentSaveData.saveMetaData.saveName;
                    string backgroundTitle = mostRecentSaveData.saveMetaData.autoSave ? "AUTOSAVE FILE" : "SAVE FILE";
                    string description = mostRecentSaveData.saveMetaData.description;


                    chapterLoadSaveButton.title.text = "CONTINUE";
                    chapterLoadSaveButton.backgroundTitle.text = backgroundTitle;
                    chapterLoadSaveButton.desc.text = description;
                    chapterLoadSaveButton.thumbnail.sprite = saveManager.levelImages[mostRecentSaveData.playerGameData.currentLevelThumbnailIndex];

                    chapterLoadSaveButton.button.onClick.AddListener(() => AddLoadGameString(mostRecentSaveData.playerGameData.currentLevelName));

                    chapterLoadGameObject.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning("No save files found to create a chapter load button.");
        }
    }


    private void CreateLoadButtons()
    {
        // Clear existing buttons first
        foreach (Transform child in loadedFileParent)
        {
            Destroy(child.gameObject);
        }



        foreach (var saveFile in saveManager.saveFiles)
        {
            SaveData data = saveManager.Load(saveFile);
            GameObject buttonObj = Instantiate(loadPrefab, loadedFileParent);
            LoadSaveButton loadSaveButton = buttonObj.GetComponent<LoadSaveButton>();

            if (loadSaveButton != null)
            {
                string saveName = data.saveMetaData.saveName;
                string title = data.saveMetaData.autoSave ? "Autosave" : "Save";
                string description = data.saveMetaData.description;

                loadSaveButton.title.text = title;
                loadSaveButton.desc.text = description;
                loadSaveButton.thumbnail.sprite = saveManager.levelImages[data.playerGameData.currentLevelThumbnailIndex];


                // Assign callbacks to the buttons
                loadSaveButton.button.onClick.AddListener(() => LoadGame(saveName));
                loadSaveButton.deleteButton.onClick.AddListener(() => AddDeleteGameString(saveName));
                loadSaveButton.deleteButton.onClick.AddListener(() => deleteSaveFileWindow.ModalWindowIn());
            }
        }
    }

    private void LoadGame(string saveName)
    {
        Debug.Log("Loading game: " + saveName);
        // Load the game here using saveManager
        // Example: saveManager.Load<GameData>(saveName);
    }

    private void LoadChapter(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void AddLoadGameString(string chapterName)
    {
        tempLoadFileName = chapterName;
    }

    public void RemoveLoadGameString()
    {
        tempLoadFileName = "";
    }

    public void LoadTempGameFile()
    {
        LoadChapter(tempLoadFileName);
    }


    public void AddDeleteGameString(string saveName)
    {
        tempDeleteFileName = saveName;
    }

    public void RemoveDeleteGameString()
    {
        tempDeleteFileName = "";
    }

    public void DeleteTempGameFile()
    {
        DeleteGame(tempDeleteFileName);
    }

    private void DeleteGame(string saveName)
    {
        saveManager.DeleteSave(saveName);
        CreateLoadButtons();
        UpdateChapterLoadButton();
    }


    private void TogglePortal()
    {
        if (isPortalOpen)
        {
            portalClose?.Invoke();
            isPortalOpen = false;
        }
        else
        {
            portalOpen?.Invoke();
            isPortalOpen = true;
        }
    }

    private void SetNewTimer()
    {
        timer = Random.Range(minRandomTime, maxRandomTime);
    }
}
