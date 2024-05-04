using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private Transform loadedFileParent;
    [SerializeField] private GameObject loadPrefab;
    [SerializeField] private string tempDeleteFileName = "";
    [SerializeField] private ModalWindowManager deleteSaveFileWindow;

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

    private void CreateLoadButtons()
    {
        // Clear existing buttons first
        foreach (Transform child in loadedFileParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var saveFile in saveManager.saveFiles)
        {
            GameObject buttonObj = Instantiate(loadPrefab, loadedFileParent);
            LoadSaveButton loadSaveButton = buttonObj.GetComponent<LoadSaveButton>();

            if (loadSaveButton != null)
            {
                string[] parts = saveFile.Split('|');
                string saveName = parts[0].Trim();
                string title = parts[1].Trim();
                string description = parts[2].Trim();

                loadSaveButton.title.text = title;
                loadSaveButton.desc.text = description;

                // Load the thumbnail if it exists
                string thumbnailPath = Application.persistentDataPath + "/saves/" + saveName + "_thumbnail.png";
                if (File.Exists(thumbnailPath))
                {
                    byte[] imageData = File.ReadAllBytes(thumbnailPath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);
                    loadSaveButton.thumbnail.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }

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
        saveManager.DeleteSave(tempDeleteFileName);
        CreateLoadButtons();

    }

    private void DeleteGame(string saveName)
    {
        saveManager.DeleteSave(saveName);
        CreateLoadButtons(); 
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
