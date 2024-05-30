using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderValueSaver : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private Slider slider;

    public UnityEvent onStart;
    public UnityEvent onSave;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
        onStart?.Invoke();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        SaveSliderSettings();
    }

    private void SaveSliderSettings()
    {
        onSave?.Invoke();
        SaveManager.Instance.SaveAsync(SaveData.Current);
    }


    #region AimSens
    public void SaveAimSensitivity()
    {
        SaveData.Current.playerProfile.aimSensitivity = slider.value;
    }

    public void GetAimSensitivity()
    {
        slider.value = SaveData.Current.playerProfile.aimSensitivity;
    }
    #endregion

    #region MasterVolume
    public void SaveMasterVolume()
    {
        SaveData.Current.playerProfile.masterVolume = slider.value;
    }

    public void GetMasterVolume()
    {
        slider.value = SaveData.Current.playerProfile.masterVolume;
    }
    #endregion

    #region MusicVolume
    public void SaveMusicVolume()
    {
        SaveData.Current.playerProfile.musicVolume = slider.value;
    }

    public void GetMusicVolume()
    {
        slider.value = SaveData.Current.playerProfile.musicVolume;
    }
    #endregion

    #region SFXVolume
    public void SaveSFXVolume()
    {
        SaveData.Current.playerProfile.effectsVolume = slider.value;
    }

    public void GetSFXVolume()
    {
        slider.value = SaveData.Current.playerProfile.effectsVolume;
    }
    #endregion


}
