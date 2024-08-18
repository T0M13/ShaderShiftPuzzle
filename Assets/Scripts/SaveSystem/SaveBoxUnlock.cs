using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveBoxUnlock : SaveBox
{
    [SerializeField] protected string unlockLevel;

    public void UnLockLevel()
    {
        if (!SaveData.Current.playerGameData.unlockedLevels.Contains(unlockLevel))
            SaveData.Current.playerGameData.unlockedLevels.Add(unlockLevel);
        SaveData.Current.playerGameData.currentLevelName = unlockLevel;
        SaveData.Current.playerGameData.currentLevelThumbnailIndex = SceneManager.GetActiveScene().buildIndex - 1 + 1; // MainMenu + NextLevel
        saveManager.SaveAsync(SaveData.Current);
        hasBeenSaved = true;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerReferences>() != null && !hasBeenSaved)
        {
            if (!SaveData.Current.playerGameData.unlockedLevels.Contains(unlockLevel))
                SaveData.Current.playerGameData.unlockedLevels.Add(unlockLevel);
            SaveData.Current.playerGameData.currentLevelName = unlockLevel;
            SaveData.Current.playerGameData.currentLevelThumbnailIndex = SceneManager.GetActiveScene().buildIndex - 1 + 1; // MainMenu + NextLevel
            saveManager.SaveAsync(SaveData.Current);
            hasBeenSaved = true;
        }
    }

}
