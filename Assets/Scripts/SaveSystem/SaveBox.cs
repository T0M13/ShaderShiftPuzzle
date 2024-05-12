using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveBox : MonoBehaviour
{
    [SerializeField] protected SaveManager saveManager;
    [SerializeField] protected bool hasBeenSaved = false;
    public SaveManager SaveManager
    {
        get => saveManager;
        private set => saveManager = value;
    }

    protected virtual void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager")?.GetComponent<SaveManager>();
        if (saveManager == null)
        {
            Debug.LogError("SaveBox " + this.gameObject + " : No saveManager object found! ");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerReferences>() != null && !hasBeenSaved)
        {
            SaveData.Current.playerGameData.currentLevelName = SceneManager.GetActiveScene().name;
            SaveData.Current.playerGameData.currentLevelThumbnailIndex = SceneManager.GetActiveScene().buildIndex - 1; // -1 Because of MainMenu
            saveManager.SaveAsync(null, SaveData.Current, true);
            hasBeenSaved = true;
        }
    }

}
