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
    public float musicStartDelay = 1f;
    private Coroutine musicCoroutine;

    private List<SoundPlayer> activeSoundPlayers = new List<SoundPlayer>();
    private MusicPlayer currentMusicPlayer;
    private int currentTrackIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentMusicPlayer = gameObject.AddComponent<MusicPlayer>();
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
        currentMusicPlayer.Initialize(track, musicMixerGroup, currentMusicPlayer.GetComponent<AudioSource>());
        currentMusicPlayer.Play();

        if (track.endTime > 0)
        {
            StartCoroutine(StopMusicAfterTime(currentMusicPlayer, track.endTime));
        }
        else
        {
            musicCoroutine = StartCoroutine(PlayNextTrack(levelMusic, currentMusicPlayer));
        }
    }

    private IEnumerator PlayNextTrack(LevelMusic levelMusic, MusicPlayer currentMusicPlayer)
    {
        while (true)
        {
            yield return new WaitForSeconds(currentMusicPlayer.GetComponent<AudioSource>().clip.length - currentMusicPlayer.sound.fadeOutDuration * 2);

            currentTrackIndex = (currentTrackIndex + 1) % levelMusic.musicTracks.Length;
            PlayMusicTrack(levelMusic, currentTrackIndex);

            currentMusicPlayer.Stop();
        }
    }

    public void StopMusic()
    {
        if (currentMusicPlayer != null)
        {
            currentMusicPlayer.Stop();
        }
    }

    private IEnumerator StopSoundAfterTime(SoundPlayer soundPlayer, float endTime)
    {
        float remainingTime = endTime - soundPlayer.GetComponent<AudioSource>().time;
        yield return new WaitForSeconds(remainingTime - soundPlayer.sound.fadeOutDuration);
        soundPlayer.Stop();
        activeSoundPlayers.Remove(soundPlayer);
    }

    private IEnumerator StopMusicAfterTime(MusicPlayer musicPlayer, float endTime)
    {
        float remainingTime = endTime - musicPlayer.GetComponent<AudioSource>().time;
        yield return new WaitForSeconds(remainingTime - musicPlayer.sound.fadeOutDuration);
        musicPlayer.Stop();
    }
}
