using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveHelpPad : InteractableObject
{
    [SerializeField][TextArea] private List<string> taskMessages = new List<string>();
    [SerializeField] private float messageDuration = 5f;
    [SerializeField] private int currentMessageIndex = 0;
    [SerializeField] private float helpEnableDelay = 20f;
    [SerializeField][ShowOnly] private float helpTimer;
    [SerializeField][ShowOnly] private bool helpEnabled = false;
    [SerializeField][ShowOnly] private bool stopTimer = false;
    [SerializeField][ShowOnly] private bool interacting = false;
    [SerializeField][ShowOnly] private Coroutine helpTimerCoroutine;
    public GameObject[] helpParticles;
    public Collider helpPadCollider;

    [SerializeField] private bool enableOnStart = true;

    [SerializeField] private Canvas helpCanvas;
    [SerializeField] private Slider helpSlider;
    [SerializeField] private TextMeshProUGUI helpCountText;

    private void Awake()
    {
        if (helpParticles != null)
            helpPadCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (enableOnStart)
        {
            EnableHelppad();
        }
        helpCanvas.gameObject.SetActive(false);
        helpTimer = helpEnableDelay;
        helpTimerCoroutine = StartCoroutine(HelpTimerCoroutine());
    }

    public void DeactivateHelper()
    {
        SetEmpty();
        DisableHelppad();
        stopTimer = true;
        StopCoroutine(helpTimerCoroutine);
    }

    public override void Interact(PlayerReferences playerRef)
    {
        if (interacting) return;
        if (taskMessages.Count == 0)
        {
            Debug.LogWarning("Task messages list is empty.");
            return;
        }

        StopAllCoroutines();
        ChangeTaskMessage(taskMessages[currentMessageIndex]);
        DisableParticles();
        StartCoroutine(DisplayMessageSequence());
        interacting = true;
    }

    private IEnumerator DisplayMessageSequence()
    {
        helpCanvas.gameObject.SetActive(true);
        helpCountText.gameObject.SetActive(true);
        helpCountText.text = (currentMessageIndex + 1)  + "/" + taskMessages.Count;

        float elapsedTime = 0f;

        while (elapsedTime < messageDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / messageDuration;
            SetHelpSliderValue(progress);

            yield return null;
        }

        SetHelpSliderValue(1f);



        yield return new WaitForSeconds(messageDuration);
        ResetHelpSlider();
        helpCountText.gameObject.SetActive(false);
        helpCountText.text = "";

        ResetTaskMessage();
        if (currentMessageIndex < taskMessages.Count - 1)
        {
            currentMessageIndex++;
        }
        interacting = false;
        helpCanvas.gameObject.SetActive(false);
    }


    public void ChangeTaskMessage(string message)
    {
        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ChangeTaskMessage(message);
        else
            Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    }

    public void ResetTaskMessage()
    {
        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ChangeTaskMessage("");
        else
            Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    }

    public void UpdateParticles(bool value)
    {
        foreach (var particle in helpParticles)
        {
            particle.SetActive(value);
        }
    }

    public void EnableHelppad()
    {
        helpEnabled = true;
        helpPadCollider.enabled = true;
        UpdateParticles(true);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySound("AuraBell", gameObject);
        }
    }

    public void DisableHelppad()
    {
        helpEnabled = false;
        helpPadCollider.enabled = false;
        UpdateParticles(false);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.StopSound(gameObject);
        }
    }

    public void DisableParticles()
    {
        UpdateParticles(false);
        if (AudioManager.Instance)
        {
            AudioManager.Instance.StopSound(gameObject);
        }
    }

    public override void SetEmpty()
    {
        base.SetEmpty();
        ResetTaskMessage();
    }

    private IEnumerator HelpTimerCoroutine()
    {
        while (!stopTimer)
        {
            if (helpTimer > 0)
            {
                helpTimer -= Time.deltaTime;
                if (helpTimer <= 0)
                {
                    EnableHelppad();
                }
            }
            yield return null;
        }
    }


    public void SetHelpSliderValue(float value)
    {
        if (!helpSlider.gameObject.activeSelf)
            helpSlider.gameObject.SetActive(true);
        if (helpSlider != null)
            helpSlider.value = value;
    }

    public void ResetHelpSlider()
    {
        if (helpSlider != null)
            helpSlider.value = 0;

        helpSlider.gameObject.SetActive(false);

    }
}