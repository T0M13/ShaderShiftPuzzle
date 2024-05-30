using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchToggleSaver : MonoBehaviour
{
    [SerializeField] private SwitchManager toggle;
    private void Start()
    {
        if (toggle == null)
            toggle = GetComponent<SwitchManager>();
        toggle.isOn = SaveData.Current.playerProfile.reverseMouse;
    }

    public void SetToggleOff()
    {
        SaveData.Current.playerProfile.reverseMouse = false;
        SaveToggleSettings();
    }

    public void SetToggleOn()
    {
        SaveData.Current.playerProfile.reverseMouse = true;
        SaveToggleSettings();
    }

    private void SaveToggleSettings()
    {
        SaveManager.Instance.SaveAsync(SaveData.Current);
    }

}
