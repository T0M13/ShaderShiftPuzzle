using System;
using System.Collections;
using UnityEngine;

namespace tomi.SaveSystem
{
    [System.Serializable]
    public class SaveMetaData
    {
        public string saveName;
        public string description;
        public bool autoSave;
    }
}