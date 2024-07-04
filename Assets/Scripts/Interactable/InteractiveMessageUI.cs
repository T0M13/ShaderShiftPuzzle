using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class InteractiveMessageUI : MonoBehaviour
{
    public static InteractiveMessageUI Instance;

    [SerializeField] private TextMeshProUGUI messageUI;
    [SerializeField] private TextMeshProUGUI useButtonUI;
    [SerializeField] private TextMeshProUGUI taskMessageUI;
    [SerializeField] private Slider chargeSlider;

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
        if (chargeSlider == null)
        {
            Debug.LogWarning("Charge Slider Reference Missing");
        }
    }

    private void Start()
    {
        ChangeMessage("");
        ChangePressMessage("");
        ChangeTaskMessage("");
        ResetChargeSlider();
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

    public void SetChargeValue(float value)
    {
        if (!chargeSlider.gameObject.activeSelf)
            chargeSlider.gameObject.SetActive(true);
        if (chargeSlider != null)
            chargeSlider.value = value;
    }

    public void ResetChargeSlider()
    {
        if (chargeSlider != null)
            chargeSlider.value = 0;

        chargeSlider.gameObject.SetActive(false);

    }
}
