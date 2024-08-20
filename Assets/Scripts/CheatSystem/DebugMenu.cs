using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu instance { get; private set; }

    [SerializeField] private GameObject menu;
    [SerializeField] private CheatConsole cheatConsole;

    [SerializeField] KeyCode toggleMenu = KeyCode.Delete;
    [SerializeField] KeyCode toggleInput = KeyCode.Tab;

    private bool isOpen;

    private void Start()
    {
        menu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleMenu)) ToggleMenu();
        if (Input.GetKeyDown(toggleInput) && isOpen) cheatConsole.EnablePanel();

    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        menu.SetActive(isOpen);
        cheatConsole.DisablePanel();
        if (GameManager.Instance)
            GameManager.Instance.ToggleCheatMenu(isOpen);
    }
}
