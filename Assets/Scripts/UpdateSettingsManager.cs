using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;

public class UpdateSettingsManager : MonoBehaviour
{
    [SerializeField] private SliderValueSaver[] sliders;
    [SerializeField] private QualityManager qualityManager;

    private void Start()
    {
        UpdateSettings();
        UpdateVsync();
    }

    private void UpdateSettings()
    {
        foreach (var slider in sliders)
        {
            slider.onStart?.Invoke();
        }
    }

    private void UpdateVsync()
    {
        qualityManager.VsyncSet(SaveData.Current.playerProfile.vsync);
        Debug.Log("Vsync: " +QualitySettings.vSyncCount);
    }
}
