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
        public float aimSensitivity;
        public bool reverseMouse;

        [Header("Quality")]
        [Range(-.5f, .5f)] public float gamma;
        [Range(-.5f, .5f)] public float brightness;
        public int vsync;
        public int resolutionValue;
        public int windowModeValue;


        public PlayerProfile()
        {
            masterVolume = 0.5f;
            musicVolume = 0.5f;
            effectsVolume = 0.5f;

            aimSensitivity = 200f;
            reverseMouse = false;

            gamma = 0;
            brightness = 0;

            vsync = 1;
        }
    }
}