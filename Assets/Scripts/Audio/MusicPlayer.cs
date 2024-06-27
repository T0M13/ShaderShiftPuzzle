using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioManager.Sound sound;

    public void Initialize(AudioManager.Sound sound, AudioMixerGroup mixerGroup, AudioSource source = null)
    {
        this.sound = sound;
        audioSource = source ? source : gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.time = sound.startTime;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.spatialBlend = sound.spatialBlend;
        audioSource.loop = sound.loop;
    }


    public void Play()
    {
        StartCoroutine(FadeIn(audioSource, sound.fadeInDuration, sound.volume));
    }

    public void Stop()
    {
        StartCoroutine(FadeOutAndStop(audioSource, sound.fadeOutDuration));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        if (duration <= 0)
        {
            audioSource.volume = targetVolume;
            audioSource.Play();
            yield break;
        }

        float currentTime = 0;
        audioSource.volume = 0;
        audioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
            yield return null;
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        if (duration <= 0)
        {
            audioSource.volume = 0.0f;
            yield break;
        }

        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float duration)
    {
        yield return StartCoroutine(FadeOut(audioSource, duration));
        audioSource.Stop();
        Destroy(this);
    }
}
