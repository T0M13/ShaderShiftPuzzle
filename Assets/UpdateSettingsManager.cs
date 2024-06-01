using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSettingsManager : MonoBehaviour
{
    [SerializeField] private SliderValueSaver[] sliders;

    private void Start()
    {
        UpdateSettings();
    }

    private void UpdateSettings()
    {
        foreach (var slider in sliders)
        {
            slider.onStart?.Invoke();
        }
    }
}
