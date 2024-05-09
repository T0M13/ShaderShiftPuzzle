using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tomi.SaveSystem
{
    [System.Serializable]
    public class PlayerGameData
    {
        public List<string> unlockedLevels = new List<string>();
        public string currentLevelName;

        public PlayerGameData()
        {
            unlockedLevels = new List<string>();
            unlockedLevels.Add("Level0");
        }
    }
}