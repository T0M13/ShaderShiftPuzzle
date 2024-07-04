﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;
using tomi.SaveSystem;

namespace Michsky.UI.Dark
{
    public class QualityManager : MonoBehaviour
    {
        //SaveManager
        [SerializeField] private SaveManager saveManager;

        // Audio
        public AudioMixer mixer;
        public SliderManager masterSlider;
        public SliderManager musicSlider;
        public SliderManager sfxSlider;

        // Resolution
        public TMP_Dropdown defaultDropdown;
        public DropdownManager customDropdown;
        public DynamicRes clickEvent;

        // Settings
        public bool preferCustomDropdown = false;
        public bool isMobile = false;

        [System.Serializable] public class DynamicRes : UnityEvent<int> { }
        [SerializeField][ShowOnly] Resolution[] resolutions;

        void Start()
        {

            saveManager = GameObject.FindGameObjectWithTag("SaveManager")?.GetComponent<SaveManager>();
            if (saveManager == null)
            {
                Debug.LogError("MainMenuManager: No saveManager object found! ");
            }

            // Audio settings
            if (mixer != null && masterSlider != null) { mixer.SetFloat("Master", Mathf.Log10(SaveData.Current.playerProfile.masterVolume) * 20); }
            if (mixer != null && musicSlider != null) { mixer.SetFloat("Music", Mathf.Log10(SaveData.Current.playerProfile.musicVolume) * 20); }
            if (mixer != null && sfxSlider != null) { mixer.SetFloat("SFX", Mathf.Log10(SaveData.Current.playerProfile.effectsVolume) * 20); }

            if (isMobile == false)
            {
                resolutions = Screen.resolutions;

                #region Irrelevant

                //                if (preferCustomDropdown == true)
                //                {
                //                    if (defaultDropdown != null)
                //                        defaultDropdown.gameObject.SetActive(false);

                //                    if (customDropdown != null) { customDropdown.gameObject.SetActive(true); }
                //                    else { return; }

                //                    customDropdown.dropdownItems.Clear();

                //                    List<string> options = new List<string>();

                //                    int currentResolutionIndex = 0;
                //                    for (int i = 0; i < resolutions.Length; i++)
                //                    {
                //#if UNITY_2022_2_OR_NEWER
                //                        string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "hz";
                //#else
                //                string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "hz";
                //#endif
                //                        customDropdown.CreateNewItem(option, null);

                //                        if (resolutions[i].width == Screen.currentResolution.width
                //                            && resolutions[i].height == Screen.currentResolution.height)
                //                            currentResolutionIndex = i;
                //                    }

                //                    customDropdown.selectedItemIndex = currentResolutionIndex;
                //                    customDropdown.UpdateValues();
                //                }
                //else
                //{
                #endregion

                if (customDropdown != null)
                    customDropdown.gameObject.SetActive(false);

                if (defaultDropdown != null) { defaultDropdown.gameObject.SetActive(true); }
                else { return; }

                defaultDropdown.ClearOptions();

                List<string> options = new List<string>();

                int currentResolutionIndex = 0;
                for (int i = 0; i < resolutions.Length; i++)
                {
#if UNITY_2022_2_OR_NEWER
                    string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "hz";
#else
                string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "hz";
#endif
                    options.Add(option);
                    //if (resolutions[i].width == Screen.currentResolution.width
                    //    && resolutions[i].height == Screen.currentResolution.height)
                    //    currentResolutionIndex = i;

                    if (resolutions[i].width == SaveData.Current.playerProfile.currentResolutionWidth
                       && resolutions[i].height == SaveData.Current.playerProfile.currentResolutionHeight)
                        currentResolutionIndex = i;

                }

                defaultDropdown.AddOptions(options);
                defaultDropdown.onValueChanged.AddListener(SetResolution);
                defaultDropdown.value = currentResolutionIndex;
                defaultDropdown.RefreshShownValue();
                //}
            }

            switch (SaveData.Current.playerProfile.currentScreenMode)
            {
                case FullScreenMode.FullScreenWindow:
                    WindowFullscreen();
                    break;
                case FullScreenMode.MaximizedWindow:
                    WindowBorderless();
                    break;
                case FullScreenMode.Windowed:
                    WindowWindowed();
                    break;
                default:
                    WindowWindowed();
                    break;
            }
        }


        public void UpdateResolution()
        {
            //if (preferCustomDropdown == true)
            //{
            //    clickEvent.Invoke(customDropdown.index);
            //    customDropdown.UpdateValues();
            //}

            //else
            //{
            //clickEvent.Invoke(defaultDropdown.value);
            SetResolution(defaultDropdown.value);
            defaultDropdown.RefreshShownValue();
            //}

            StartCoroutine("FixResolution");
        }

        public void SetResolution(int resolutionIndex)
        {
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreenMode);

            SaveData.Current.playerProfile.currentResolutionWidth = resolutions[resolutionIndex].width;
            SaveData.Current.playerProfile.currentResolutionHeight = resolutions[resolutionIndex].height;

            saveManager.SaveAsync(SaveData.Current);


        }

        public void AnisotrpicFilteringSet(int index)
        {
            if (index == 0)
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            else if (index == 1)
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            else if (index == 2)
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }

        public void AntiAlisasingSet(int index)
        {
            // 0, 2, 4, 8 - Zero means off
            QualitySettings.antiAliasing = index;
        }

        public void VsyncSet(int index)
        {
            // 0, 1 - Zero means off
            QualitySettings.vSyncCount = index;
        }

        public void ShadowResolutionSet(int index)
        {
            if (index == 3) { QualitySettings.shadowResolution = ShadowResolution.VeryHigh; }
            else if (index == 2) { QualitySettings.shadowResolution = ShadowResolution.High; }
            else if (index == 1) { QualitySettings.shadowResolution = ShadowResolution.Medium; }
            else if (index == 0) { QualitySettings.shadowResolution = ShadowResolution.Low; }
        }

        public void ShadowsSet(int index)
        {
            if (index == 0) { QualitySettings.shadows = ShadowQuality.Disable; }
            else if (index == 1) { QualitySettings.shadows = ShadowQuality.All; }
        }

        public void ShadowsCascasedSet(int index)
        {
            //0 = No, 2 = Two, 4 = Four
            QualitySettings.shadowCascades = index;
        }

        public void TextureSet(int index)
        {
            // 0 = Full, 4 = Eight Resolution
#if UNITY_2022_2_OR_NEWER
            QualitySettings.globalTextureMipmapLimit = index;
#else
            QualitySettings.masterTextureLimit = index;
#endif
        }

        public void SoftParticleSet(int index)
        {
            if (index == 0) { QualitySettings.softParticles = false; }
            else if (index == 1) { QualitySettings.softParticles = true; }
        }

        public void ReflectionSet(int index)
        {
            if (index == 0) { QualitySettings.realtimeReflectionProbes = false; }
            else if (index == 1) { QualitySettings.realtimeReflectionProbes = true; }
        }

        public void VolumeSetMaster(float volume) { mixer.SetFloat("Master", Mathf.Log10(volume) * 20); }
        public void VolumeSetMusic(float volume) { mixer.SetFloat("Music", Mathf.Log10(volume) * 20); }
        public void VolumeSetSFX(float volume) { mixer.SetFloat("SFX", Mathf.Log10(volume) * 20); }

        public void SetOverallQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void WindowFullscreen()
        {
            Screen.fullScreen = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            SaveData.Current.playerProfile.currentScreenMode = FullScreenMode.FullScreenWindow;
            saveManager.SaveAsync(SaveData.Current);
        }

        public void WindowBorderless()
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;

            SaveData.Current.playerProfile.currentScreenMode = FullScreenMode.MaximizedWindow;
            saveManager.SaveAsync(SaveData.Current);
        }

        public void WindowWindowed()
        {
            Screen.fullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;

            SaveData.Current.playerProfile.currentScreenMode = FullScreenMode.Windowed;
            saveManager.SaveAsync(SaveData.Current);
        }
    }
}