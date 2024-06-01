using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tomi.SaveSystem
{
    [System.Serializable]
    public class PlayerProfile
    {
        [Header("Settings")]
        [Header("Audio")]
        public float masterVolume;
        public float musicVolume;
        public float effectsVolume;

        [Header("Sensitivity")]
        [Range(1, 100)] public float aimSensitivity;
        public bool reverseMouse;

        [Header("Quality")]
        [Range(1, 100)] public float gamma;
        [Range(1, 100)] public float brightness;


        public PlayerProfile()
        {
            masterVolume = 0.5f;
            musicVolume = 0.5f;
            effectsVolume = 0.5f;

            aimSensitivity = 20f;
            reverseMouse = false;

            gamma = 0;
            brightness = 0;
        }
    }
}