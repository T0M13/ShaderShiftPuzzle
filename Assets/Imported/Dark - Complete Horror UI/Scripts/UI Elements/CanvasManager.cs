using Michsky.UI.Dark;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public MainPanelManager mainPanelManager;
    public CanvasScaler canvasScaler;
    public Transform[] transformsToActivateOnPause;
    public Transform[] transformsToDeactivateOnPause;

    void Start()
    {
        if (canvasScaler == null)
            canvasScaler = gameObject.GetComponent<CanvasScaler>();
    }

    public void ScaleCanvas(int scale = 1080)
    {
        canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.x, scale);
    }

    public void OnToggleCanvas(bool value)
    {
        foreach (var item in transformsToActivateOnPause)
        {
            item.gameObject.SetActive(value);
            mainPanelManager.OpenFirstTab();
            mainPanelManager.EnableFirstPanel();
        }

        foreach (var item in transformsToDeactivateOnPause)
        {
            item.gameObject.SetActive(!value);
        }
    }

}
