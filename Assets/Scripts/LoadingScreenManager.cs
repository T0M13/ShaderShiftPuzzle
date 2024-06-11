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

    public Action onBeforeLoadingScreen;
    public Action onAfterLoadingScreen;

    [System.Serializable]
    private class LevelBackgrounds
    {
        public string levelName;
        public Sprite[] backgroundImage;
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
        foreach (var levelBackground in levelBackgrounds)
        {
            if (levelBackground.levelName == levelName)
            {
                if (levelBackground.backgroundImage.Length > 1)
                {
                    Debug.Log("Multiple sprites available.");
                }
                else
                {
                    Debug.Log("Only one sprite available.");
                }

                int index = UnityEngine.Random.Range(0, levelBackground.backgroundImage.Length);
                Debug.Log($"Selected sprite index: {index}");
                backgroundImage.sprite = levelBackground.backgroundImage[index];
                return;
            }
        }
        Debug.LogWarning("No background found for level: " + levelName);
    }

}
