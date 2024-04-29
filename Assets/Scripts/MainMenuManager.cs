using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    [Header("Events to Execute on Start")]
    public UnityEvent eventsOnStart;

    [Header("Portal")]
    [SerializeField] private float minRandomTime = 20;
    [SerializeField] private float maxRandomTime = 40;
    [SerializeField][ShowOnly] private float timer;
    [SerializeField][ShowOnly] private bool isPortalOpen = false;

    public UnityEvent portalOpen;
    public UnityEvent portalClose;

    private void Start()
    {
        eventsOnStart?.Invoke();
        SetNewTimer();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TogglePortal();
            SetNewTimer();
        }
    }

    private void TogglePortal()
    {
        if (isPortalOpen)
        {
            portalClose?.Invoke();
            isPortalOpen = false;
        }
        else
        {
            portalOpen?.Invoke();
            isPortalOpen = true;
        }
    }

    private void SetNewTimer()
    {
        timer = Random.Range(minRandomTime, maxRandomTime);
    }
}
