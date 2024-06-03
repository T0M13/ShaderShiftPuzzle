using Michsky.UI.Dark;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{

    public static LoadingScreenManager instance;
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField] private Slider progressbar;
    [SerializeField] private UIDissolveEffect uIDissolveEffect;
    [SerializeField] private float beforeWaitTime = 1f;
    [SerializeField] private float additionalWaitTime = 0.2f;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private List<LevelBackgrounds> levelBackgrounds = new List<LevelBackgrounds>();
    private Dictionary<string, Sprite> levelBackgroundsDic = new Dictionary<string, Sprite>();


    public Action onBeforeLoadingScreen;
    public Action onAfterLoadingScreen;

    [System.Serializable]
    private class LevelBackgrounds
    {
        public string levelName;
        public Sprite backgroundImage;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        loadingScreenUI.SetActive(false);
        InitializeLevelBackgrounds();

    }

    private void InitializeLevelBackgrounds()
    {
        foreach (var levelbackground in levelBackgrounds)
        {
            levelBackgroundsDic.Add(levelbackground.levelName, levelbackground.backgroundImage);
        }
    }

    public void SwitchToScene(int id)
    {
        loadingScreenUI.SetActive(true);
        progressbar.value = 0;
        StartCoroutine(SwitchToSceneAsync(id));
    }

    IEnumerator SwitchToSceneAsync(int id)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        while (!asyncLoad.isDone)
        {
            progressbar.value = asyncLoad.progress;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        loadingScreenUI.SetActive(false);
    }

    public void SwitchToScene(string levelName)
    {
        StartCoroutine(SwitchToSceneAsync(levelName));
    }
    IEnumerator SwitchToSceneAsync(string levelName)
    {
        uIDissolveEffect.gameObject.SetActive(true);
        uIDissolveEffect.DissolveIn();
        yield return new WaitForSeconds(beforeWaitTime);
        onBeforeLoadingScreen?.Invoke();
        loadingScreenUI.SetActive(true);
        progressbar.value = 0;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone)
        {
            progressbar.value = asyncLoad.progress;
            yield return null;
        }
        yield return new WaitForSeconds(additionalWaitTime);
        loadingScreenUI.SetActive(false);
        onAfterLoadingScreen?.Invoke();
        uIDissolveEffect.DissolveOut();
    }

    public void ChangeBackground(string levelName)
    {
        if (levelBackgrounds.Count == 0)
        {
            Debug.LogWarning("No background images set in the LoadingScreenManager.");
            return;
        }

        if (levelBackgroundsDic.TryGetValue(levelName, out Sprite backgroundImageSprite))
        {
            backgroundImage.sprite = backgroundImageSprite;
        }
        else
        {
            Debug.LogWarning("No background found for level: " + levelName);
        }

    }

}
