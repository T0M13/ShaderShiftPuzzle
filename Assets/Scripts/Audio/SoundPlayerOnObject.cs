using UnityEngine;

public class SoundPlayerOnObject : MonoBehaviour
{
    [SerializeField]
    private string soundName;
    public string SoundName { get => soundName; set => soundName = value; }
    public GameObject SoundObject { get => soundObject; set => soundObject = value; }

    [SerializeField]
    private GameObject soundObject;
    [SerializeField]
    private bool onAwake = false;

    private void Start()
    {
        AudioManager.Instance.InitializeSound(SoundName, SoundObject);
        if (!onAwake) return;
        PlaySelectedSound();
    }

    public void PlaySelectedSound()
    {
        if (!string.IsNullOrEmpty(SoundName))
        {
            AudioManager.Instance.PlaySound(SoundName, SoundObject);
        }
        else
        {
            Debug.LogWarning("Sound name is not set.");
        }
    }

}
