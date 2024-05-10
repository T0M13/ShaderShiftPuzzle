using Michsky.UI.Dark;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public MainPanelManager mainPanelManager;
    public CanvasScaler canvasScaler;
    public Transform[] transformsToActivateOnPause;
    public Transform[] transformsToDeactivateOnPause;

    [Header("Modal Buttons")]
    public Button exitGameButton;
    public Button exitMainMenuButton;
    public Button restartButton;
    public Button resumeButton;

    [Header("Other References")]
    public UIDissolveEffect uiDissolveEffect;

    public UnityEvent onLevelStart;


    void Start()
    {
        if (canvasScaler == null)
            canvasScaler = gameObject.GetComponent<CanvasScaler>();

        onLevelStart?.Invoke();
    }
    public void SetButtonFunctions(GameManager gameManager)
    {
        exitGameButton.onClick.AddListener(gameManager.ExitGame);
        exitMainMenuButton.onClick.AddListener(gameManager.BackToMainMenu);
        restartButton.onClick.AddListener(gameManager.RestartLevel);
        resumeButton.onClick.AddListener(gameManager.OnPause);
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
        }
        mainPanelManager.OpenFirstTab();
        mainPanelManager.ToggleFirstPanel(value);

        foreach (var item in transformsToDeactivateOnPause)
        {
            item.gameObject.SetActive(!value);
        }
    }

}
