using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineEndHandler : MonoBehaviour
{
    private PlayableDirector playableDirector;

    public UnityEvent onTimeLineStart;
    public UnityEvent onTimeLineEnd;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
            StartTimeline();
        }
        else
        {
            Debug.LogError("PlayableDirector component not found on " + gameObject.name);
        }
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            Debug.Log("Timeline has ended");
            OnTimelineEnded();
        }
    }

    public void StartTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            Debug.Log("Timeline started");
            onTimeLineStart?.Invoke();
        }
        else
        {
            Debug.LogError("PlayableDirector component not assigned.");
        }
    }

    private void OnTimelineEnded()
    {
        Debug.Log("Timeline ended");
        onTimeLineEnd?.Invoke();
    }

    public void EndLevel()
    {
        GameManager.Instance.BackToMainMenu();
    }
}
