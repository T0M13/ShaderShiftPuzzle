using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{

    public UnityEvent onStart;

    private void Start()
    {
        onStart?.Invoke();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenLink(string feedbackLink)
    {
        Application.OpenURL(feedbackLink);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

}
