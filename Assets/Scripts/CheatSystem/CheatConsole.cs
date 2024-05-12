using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CheatConsole : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject cheatConsolePanel;

    [System.Serializable]
    public struct CommandEvent
    {
        public string command;
        public UnityEvent response;
    }

    [Header("Cheats")]
    public CommandEvent[] commands;

    public void EnablePanel()
    {
        cheatConsolePanel.SetActive(true);
        inputField.Select();
    }

    public void DisablePanel()
    {
        cheatConsolePanel.SetActive(false);
    }

    public void ProcessCheatInput(string commandInput)
    {
        commandInput = commandInput.Trim();
        foreach (var command in commands)
        {
            if (commandInput.Equals(command.command, System.StringComparison.OrdinalIgnoreCase))
            {
                command.response.Invoke();
                return;
            }
        }
        Debug.LogWarning("Command not found: " + commandInput);
    }
}
