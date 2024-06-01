using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SliderValueSaver : MonoBehaviour, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Slider slider;

    public UnityEvent onStart;
    public UnityEvent onSave;
    public UnityEvent onDrag;

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
    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke();
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

    #region Brightness
    public void SaveBrightness()
    {
        QualityProfileSettings.Instance.SetPostExposure(slider.value);
        SaveData.Current.playerProfile.brightness = slider.value;
    }

    public void GetBrightness()
    {
        slider.value = SaveData.Current.playerProfile.brightness;
        QualityProfileSettings.Instance.SetPostExposure(SaveData.Current.playerProfile.brightness);
    }

    #endregion

    #region Gamma
    public void SaveGamma()
    {
        QualityProfileSettings.Instance.SetGamma(slider.value);
        SaveData.Current.playerProfile.gamma = slider.value;
    }

    public void GetGamma()
    {
        slider.value = SaveData.Current.playerProfile.gamma;
        QualityProfileSettings.Instance.SetGamma(SaveData.Current.playerProfile.gamma);
    }

    #endregion



}
