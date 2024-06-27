using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class InteractiveHelpPad : InteractableObject
{
    [SerializeField][TextArea] private List<string> taskMessages = new List<string>();
    [SerializeField] private float messageDuration = 5f;
    [SerializeField] private int currentMessageIndex = 0;
    [SerializeField] private float helpEnableDelay = 20f;
    [SerializeField][ShowOnly] private float helpTimer;
    [SerializeField][ShowOnly] private bool helpEnabled = false;
    [SerializeField][ShowOnly] private bool stopTimer = false;
    [SerializeField][ShowOnly] private Coroutine helpTimerCoroutine;
    public GameObject[] helpParticles;
    public Collider helpPadCollider;

    [SerializeField] private bool enableOnStart = true;

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
        if (taskMessages.Count == 0)
        {
            Debug.LogWarning("Task messages list is empty.");
            return;
        }

        StopAllCoroutines();
        ChangeTaskMessage(taskMessages[currentMessageIndex]);
        DisableParticles();
        StartCoroutine(DisplayMessageSequence());
    }

    private IEnumerator DisplayMessageSequence()
    {
        yield return new WaitForSeconds(messageDuration);
        ResetTaskMessage();
        if (currentMessageIndex < taskMessages.Count - 1)
        {
            currentMessageIndex++;
        }
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
}