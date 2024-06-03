using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Chapters")]
    [SerializeField] private ChapterLockWindow[] chapterWindows;

    [Header("LoadSave Files")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LoadingScreenManager loadingScreenManager;
    [SerializeField] private Transform chapterLoadParent;
    [SerializeField] private GameObject chapterLoadGameObject;
    [SerializeField] private Transform loadedFileParent;
    [SerializeField] private GameObject loadPrefab;
    [SerializeField][ShowOnly] private string tempDeleteFileName = "";
    [SerializeField] private ModalWindowManager deleteSaveFileWindow;

    [SerializeField][ShowOnly] private string tempLoadFileName = "";
    [SerializeField][ShowOnly] private string tempLoadFilePath = "";
    [SerializeField] private ModalWindowManager loadSaveFileWindow;

    [Header("Settings")]
    [SerializeField] private SliderValueSaver[] sliders;

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
            UpdateChapters();
            UpdateChapterLoadButton();
            UpdateSettings();
            //CreateLoadButtons();
        }

        loadingScreenManager = GameObject.FindGameObjectWithTag("LoadingScreenManager")?.GetComponent<LoadingScreenManager>();
        if (loadingScreenManager == null)
        {
            Debug.LogError("MainMenuManager: No loadingScreenManager object found! ");
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

    private void UpdateSettings()
    {
        foreach (var slider in sliders)
        {
            slider.onStart?.Invoke();
        }
    }

    private void UpdateChapters()
    {
        for (int i = 0; i < chapterWindows.Length; i++)
        {
            if (i < SaveData.Current.playerGameData.unlockedLevels.Count && !string.IsNullOrEmpty(SaveData.Current.playerGameData.unlockedLevels[i]))
            {
                chapterWindows[i].chapterButton.interactable = true;
                chapterWindows[i].lockScreen.gameObject.SetActive(false);
            }
            else
            {
                chapterWindows[i].chapterButton.interactable = false;
                chapterWindows[i].lockScreen.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateChapterLoadButton()
    {
        if (!string.IsNullOrEmpty(SaveData.Current.playerGameData.currentLevelName))
        {
            ChapterLoadSaveButton chapterLoadSaveButton = chapterLoadGameObject.GetComponent<ChapterLoadSaveButton>();

            if (chapterLoadSaveButton != null)
            {
                chapterLoadSaveButton.title.text = "CONTINUE";
                chapterLoadSaveButton.backgroundTitle.text = "CONTINUE";
                chapterLoadSaveButton.desc.text = SaveData.Current.saveMetaData.description;
                chapterLoadSaveButton.thumbnail.sprite = saveManager.levelImages[SaveData.Current.playerGameData.currentLevelThumbnailIndex];

                chapterLoadSaveButton.button.onClick.AddListener(() => AddLoadGameString(SaveData.Current.playerGameData.currentLevelName));

                chapterLoadGameObject.SetActive(true);
            }
        }

        else
        {
            chapterLoadGameObject.SetActive(false);
            Debug.LogWarning("No save files found to create a chapter load button.");
        }
    }

    private void LoadChapter(string level)
    {
        if (loadingScreenManager != null)
        {
            loadingScreenManager.ChangeBackground(level);
            loadingScreenManager.SwitchToScene(level);
        }
        else
        {
            Debug.LogWarning("No LoadingScreen");
        }
    }

    public void AddLoadGameString(string chapterName)
    {
        tempLoadFileName = chapterName;
    }

    public void RemoveLoadGameString()
    {
        tempLoadFileName = "";
        tempLoadFilePath = "";
    }

    public void LoadTempGameFile()
    {
        SaveData.Current = saveManager.Load();
        LoadChapter(tempLoadFileName);
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

    [System.Serializable]
    public class ChapterLockWindow
    {
        public Button chapterButton;
        public Transform lockScreen;
    }
}
