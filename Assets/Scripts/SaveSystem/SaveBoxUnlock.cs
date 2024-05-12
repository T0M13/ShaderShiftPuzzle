using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveBoxUnlock : SaveBox
{
    [SerializeField] protected string unLockLevel;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerReferences>() != null && !hasBeenSaved)
        {
            SaveData.Current.playerGameData.unlockedLevels.Add(unLockLevel);
            SaveData.Current.playerGameData.currentLevelName = unLockLevel;
            SaveData.Current.playerGameData.currentLevelThumbnailIndex = SceneManager.GetActiveScene().buildIndex - 1 + 1; // MainMenu + NextLevel
            saveManager.SaveAsync(null, SaveData.Current, true);
            hasBeenSaved = true;
        }
    }

}
