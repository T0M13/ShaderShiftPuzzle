using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;

public class WindowModeOption : MonoBehaviour
{

    [SerializeField] private HorizontalSelector horizontalSelector;

    private void OnEnable()
    {
        UpdateWindowMode();
    }

    private void UpdateWindowMode()
    {
        switch (SaveData.Current.playerProfile.currentScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                horizontalSelector.index = 1;
                horizontalSelector.defaultIndex = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                horizontalSelector.index = 2;
                horizontalSelector.defaultIndex = 2;
                break;
            case FullScreenMode.Windowed:
                horizontalSelector.index = 0;
                horizontalSelector.defaultIndex = 0;
                break;
        }
    }
}
