using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip[] musicSounds, sfxSounds;
    private Dictionary<string, AudioClip> musicClips, sfxClips;
    [SerializeField] private AudioSource musicSource, sfxSource;
    private List<AudioSource> audioSources;
    public AudioMixer mixer;
    private bool isMusicStopping;
    private void Awake()
    {
        musicClips = musicSounds.ToDictionary(x => x.name, x => x);
        sfxClips = sfxSounds.ToDictionary(x => x.name, x => x);
    }
    private void Start()
    {
        musicSource.ignoreListenerPause = true;
        audioSources = new();
    }
    #region Volume
    public void LoadAllVolume()
    {
        LoadVolume("masterVolume", "Master");
        LoadVolume("musicVolume", "Music");
        LoadVolume("sfxVolume", "SFX");
    }
    private void LoadVolume(string key, string mixerChannel)
    {
        float value = 0.5f;
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, value);
        }
        else
            value = PlayerPrefs.GetFloat(key);
        mixer.SetFloat(mixerChannel, Mathf.Log10(value) * 20);
    }
    #endregion
    #region Music
    public void PlayMusicLoop(string name, float volumeScale = 1f, float lerpTime = 0)
    {
        musicSource.clip = musicClips[name];
        if (lerpTime == 0) musicSource.volume = volumeScale;
        else if (isMusicStopping) StartCoroutine(WaitForStartMusicLoop(lerpTime, volumeScale));
        else StartCoroutine(AudioSourceSmoothTransition(musicSource, lerpTime, true, volumeScale));
        musicSource.Play();
    }
    private IEnumerator WaitForStartMusicLoop(float lerpTime, float volumeScale)
    {
        while (isMusicStopping) yield return null;
        StartCoroutine(AudioSourceSmoothTransition(musicSource, lerpTime, true, volumeScale));
    }
    public void StopMusicLoop(float lerpTime = 0f)
    {
        if (lerpTime == 0) musicSource.Stop();
        else StartCoroutine(StoppingMusic(lerpTime));
    }
    private IEnumerator StoppingMusic(float lerpTime)
    {
        isMusicStopping = true;
        yield return StartCoroutine(AudioSourceSmoothTransition(musicSource, lerpTime));
        isMusicStopping = false;
    }
    #endregion
    public void PlaySFXOnce(string name, float volumeScale = 1f)
    {
        sfxSource.PlayOneShot(sfxClips[name], volumeScale);
    }
    public void PlaySFXOnceRandomPitch(SoundValues sound)
    {
        // Check if an AudioSource with the given clip already exists
        if (ListContainsClip(sound.sound))
        {
            AudioSource source = FindAudioSourceByClipName(sound.sound);
            source.pitch = Random.Range(sound.minPitch, sound.maxPitch);
            source.PlayOneShot(sfxClips[sound.sound], sound.volume);
        }
        else
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            source.pitch = Random.Range(sound.minPitch, sound.maxPitch);
            source.PlayOneShot(sfxClips[sound.sound], sound.volume);
        }
    }
    #region Loops
    public void PlaySFXLoop(string clipName, float volumeScale = 1f, float lerpTime = 0f)
    {
        // Find the AudioClip in the audioClips array
        AudioClip clip = sfxClips[clipName];
        if (clip == null)
            return;

        // Create AudioSource component
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        // Set up AudioSource to play the clip on loop
        source.clip = clip;
        source.loop = true;
        if (lerpTime == 0) source.volume = volumeScale;
        else StartCoroutine(AudioSourceSmoothTransition(source, lerpTime, true, volumeScale));
        source.Play();

        // Add the AudioSource to the array
        audioSources.Add(source);
    }

    public void StopLoop(string clipName, float lerpTime = 0f)
    {
        // Find the AudioSource with the given clipName
        AudioSource source = FindAudioSourceByClipName(clipName);

        // If AudioSource is found, stop and remove it
        if (source != null)
        {
            if (lerpTime == 0) Destroy(source);
            else StartCoroutine(AudioSourceSmoothTransition(source, lerpTime));
        }
        else Debug.Log("Couldn't stop audio " + clipName + " because it's not playing");
    }
    public void StopAllLoops(float lerpTime = 0f)
    {
        foreach(AudioSource source in audioSources.ToArray())
        {
            StartCoroutine(AudioSourceSmoothTransition(source, lerpTime));
        }
        audioSources = new();
    }
    private IEnumerator AudioSourceSmoothTransition(AudioSource source, float lerpTime, bool isSmoothStart = false, float targetVolume = 1f)
    {
        float t = 0;
        float startVolume = isSmoothStart ? 0f : source.volume;
        float endVolume = isSmoothStart ? targetVolume : 0f;

        while (t < lerpTime && source != null)
        {
            t += Time.unscaledDeltaTime;
            float value = t / lerpTime;
            source.volume = Mathf.SmoothStep(startVolume, endVolume, value);
            yield return null;
        }
        if (!isSmoothStart && source != musicSource)
        {
            audioSources.Remove(source);
            Destroy(source);
        }
    }
    public void CreateAudioSource(Transform obj, SoundValues sound)
    {
        GameObject audioGameObject = new();
        AudioSource audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 3;
        audioSource.maxDistance = 40;
        if (sound.maxPitch != -4 && sound.minPitch != -4)
            audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        audioGameObject.transform.Translate(obj.position);
        AudioClip clipToPlay = sfxClips[sound.sound];
        audioSource.volume = sound.volume;
        audioSource.clip = clipToPlay;
        audioSource.time = sound.startTime;
        audioSource.Play();
        Destroy(audioGameObject, clipToPlay.length);
    }
    #endregion
    #region Pitch Change SFX
    private int pitchRiseCount;
    private float pitchTime;
    public void PlaySFXRisingPitch(string name, float volumeScale, float pitchRise = 0.3f, float time = 1.5f, int maxPitchRiseCount = 3)
    {
        // Check if an AudioSource with the given clip already exists
        if (ListContainsClip(name))
        {
            AudioSource source = FindAudioSourceByClipName(name);
            if (pitchRiseCount <= maxPitchRiseCount) source.pitch = 1 + pitchRiseCount * pitchRise;
            pitchRiseCount++;
            source.Play();
            source.volume = volumeScale;
            pitchTime += time;
        }
        else
        {
            pitchRiseCount = 1;
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            source.clip = sfxClips[name];
            source.Play();
            StartCoroutine(WaitToDestroySource(source, time));
        }
    }
    private IEnumerator WaitToDestroySource(AudioSource source, float startTime)
    {
        audioSources.Add(source);
        pitchTime = startTime;
        float t = 0;
        while (t < pitchTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        audioSources.Remove(source);
        Destroy(source);
    }
    #endregion
    #region Helpers
    // Helper method to check if audioSources array contains an AudioSource with the given clip name
    private bool ListContainsClip(string clipName)
    {
        foreach (var source in audioSources)
        {
            if (source != null && source.clip != null && source.clip.name == clipName)
                return true;
        }
        return false;
    }
    // Helper method to find AudioSource by clip name
    private AudioSource FindAudioSourceByClipName(string clipName)
    {
        foreach (var source in audioSources)
        {
            if (source != null && source.clip != null && source.clip.name == clipName)
                return source;
        }
        return null;
    }
    #endregion
}
