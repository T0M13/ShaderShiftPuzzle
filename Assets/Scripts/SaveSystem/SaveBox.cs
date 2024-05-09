using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveBox : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private bool hasBeenSaved = true;
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
            Debug.LogError("SaveBox " + this.gameObject + " : No saveManager object found! ");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerReferences>() != null)
        {
            SaveData.Current.playerGameData.currentLevelName = SceneManager.GetActiveScene().name;
            saveManager.SaveAsync(null, SaveData.Current, true);
        }
    }

}
