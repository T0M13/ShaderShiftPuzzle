using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume = 1.0f;
        public float pitch = 1.0f;
        public float startTime = 0.0f;
        public float endTime = 0.0f;
        public float spatialBlend = 0.0f;
        public bool loop = false;
        public float fadeInDuration = 1f;
        public float fadeOutDuration = 1f;
    }

    [System.Serializable]
    public class LevelMusic
    {
        public string levelName;
        public Sound[] musicTracks;
    }

    public Sound[] soundEffects;
    public LevelMusic[] levelMusicTracks;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    private Coroutine musicCoroutine;

    private List<SoundPlayer> activeSoundPlayers = new List<SoundPlayer>();
    private AudioSource musicSource;
    private int currentTrackIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = musicMixerGroup;
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusic(scene.name);
    }

    public void PlaySound(string soundName, GameObject targetObject = null)
    {
        Sound s = System.Array.Find(soundEffects, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return;
        }

        GameObject soundObject = targetObject ?? new GameObject("Sound_" + soundName);
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        SoundPlayer soundPlayer = soundObject.AddComponent<SoundPlayer>();
        soundPlayer.Initialize(s, sfxMixerGroup, audioSource);
        soundPlayer.Play();

        activeSoundPlayers.Add(soundPlayer);

        if (s.endTime > 0 && !s.loop)
        {
            StartCoroutine(StopSoundAfterTime(soundPlayer, s.endTime));
        }
    }

    public void StopSound(GameObject targetObject)
    {
        SoundPlayer soundPlayer = targetObject.GetComponent<SoundPlayer>();
        if (soundPlayer != null)
        {
            soundPlayer.Stop();
            activeSoundPlayers.Remove(soundPlayer);
        }
    }

    public void PlayMusic(string levelName)
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        LevelMusic levelMusic = System.Array.Find(levelMusicTracks, lm => lm.levelName == levelName);
        if (levelMusic == null || levelMusic.musicTracks.Length == 0) return;

        currentTrackIndex = 0;
        PlayMusicTrack(levelMusic, currentTrackIndex);
    }

    private void PlayMusicTrack(LevelMusic levelMusic, int trackIndex)
    {
        Sound track = levelMusic.musicTracks[trackIndex];
        musicSource.clip = track.clip;
        musicSource.volume = 0; // Start at 0 volume for fade-in
        musicSource.pitch = track.pitch;
        musicSource.time = track.startTime;
        musicSource.loop = track.loop;
        musicSource.Play();

        StartCoroutine(FadeIn(musicSource, track.fadeInDuration, track.volume));

        if (track.endTime > 0)
        {
            StartCoroutine(StopMusicAfterTime(track.endTime, track.fadeOutDuration));
        }
        else
        {
            musicCoroutine = StartCoroutine(PlayNextTrack(levelMusic, track.fadeOutDuration));
        }
    }

    private IEnumerator PlayNextTrack(LevelMusic levelMusic, float fadeOutDuration)
    {
        while (true)
        {
            yield return new WaitForSeconds(musicSource.clip.length - fadeOutDuration * 2);

            StartCoroutine(FadeOut(musicSource, fadeOutDuration));
            yield return new WaitForSeconds(fadeOutDuration);

            currentTrackIndex = (currentTrackIndex + 1) % levelMusic.musicTracks.Length;
            PlayMusicTrack(levelMusic, currentTrackIndex);
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            Sound currentTrack = System.Array.Find(levelMusicTracks[currentTrackIndex].musicTracks, track => track.clip == musicSource.clip);
            if (currentTrack != null)
            {
                StartCoroutine(FadeOutAndStop(musicSource, currentTrack.fadeOutDuration));
            }
        }
    }

    private IEnumerator StopSoundAfterTime(SoundPlayer soundPlayer, float endTime)
    {
        float remainingTime = endTime - soundPlayer.GetComponent<AudioSource>().time;
        yield return new WaitForSeconds(remainingTime - soundPlayer.sound.fadeOutDuration);
        soundPlayer.Stop();
        activeSoundPlayers.Remove(soundPlayer);
    }

    private IEnumerator StopMusicAfterTime(float endTime, float fadeOutDuration)
    {
        float remainingTime = endTime - musicSource.time;
        yield return new WaitForSeconds(remainingTime - fadeOutDuration);
        StartCoroutine(FadeOutAndStop(musicSource, fadeOutDuration));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        if (duration <= 0)
        {
            audioSource.volume = targetVolume;
            yield break;
        }

        float currentTime = 0;
        audioSource.volume = 0;
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
        float startVolume = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float duration)
    {
        yield return StartCoroutine(FadeOut(audioSource, duration));
        audioSource.Stop();
    }
}
