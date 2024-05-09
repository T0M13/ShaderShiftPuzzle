using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveMessageUI : MonoBehaviour
{
    public static InteractiveMessageUI Instance;

    [SerializeField] private TextMeshProUGUI messageUI;
    [SerializeField] private TextMeshProUGUI useButtonUI;
    [SerializeField] private TextMeshProUGUI taskMessageUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (messageUI == null)
        {
            messageUI = GetComponent<TextMeshProUGUI>();
        }
        if (useButtonUI == null)
        {
            Debug.LogWarning("useButton Reference Missing");
        }
    }

    private void Start()
    {
        ChangeMessage("");
        ChangePressMessage("");
        ChangeTaskMessage("");
    }

    public void ChangeMessage(string message)
    {
        messageUI.text = message;
    }

    public void ChangePressMessage(string message)
    {
        useButtonUI.text = message;
    }

    public void ChangeTaskMessage(string message)
    {
        taskMessageUI.text = message;
    }
}
