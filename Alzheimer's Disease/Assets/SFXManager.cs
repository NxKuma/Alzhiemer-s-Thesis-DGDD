using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource[] _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    Dictionary<AudioSource, bool> fadingCoroutines = new Dictionary<AudioSource, bool>();

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {  
        foreach (AudioSource source in _audioSource)
        {
            fadingCoroutines[source] = false;
        }   
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in _audioSource)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        Debug.LogWarning("SFXManager: All audio sources are currently playing. Consider increasing the number of audio sources for better performance.");
        return _audioSource[0]; // fallback to first audio source if all are busy
    } 

    private AudioSource GetAudioSourcePlaying(string sfxName)
    {
        foreach (AudioSource source in _audioSource)
        {
            if (source.isPlaying && source.clip != null && source.clip.name == sfxName)
            {
                return source;
            }
        }
        return null;
    }

    public void PlaySFX(string sfxName, bool islooping = false)
    {
        AudioSource availAudio = GetAvailableAudioSource();
        availAudio.volume = 1f;

        if (IsSFXPlaying(sfxName, availAudio))
        {
            Debug.Log($"SFX '{sfxName}' is already playing on the selected audio source. Skipping play request.");
            return;
        }

        foreach (AudioClip clip in _audioClips)
        {
            if (clip.name.Contains(sfxName))
            {
                availAudio.clip = clip;
                if (islooping)
                {
                    availAudio.loop = true;
                    if (!availAudio.isPlaying)
                    {
                        Debug.Log($"Playing looping SFX: {sfxName}");
                        availAudio.Play();
                    }else
                    {
                        return;
                    }
                    return;
                }
                else
                {
                    Debug.Log($"Playing one-shot SFX: {sfxName}");
                    availAudio.loop = false;
                    availAudio.PlayOneShot(clip);
                    return;
                }
            }
        }
        Debug.LogWarning($"SFXManager: No audio clip found with name {sfxName}");
    }

    public void StopSFX(string sfxName)
    {
        AudioSource audioSource = GetAudioSourcePlaying(sfxName);
        if (audioSource != null)
        {
            if (audioSource.loop && !fadingCoroutines[audioSource])
            {
                Debug.Log($"Stopping looping SFX: {sfxName}");
                StartCoroutine(FadeOutSFXCoroutine(audioSource, 0.5f)); // fade out over 0.5 seconds
            }
            else
            {
                Debug.Log($"Stopping SFX: {sfxName}");
                audioSource.Stop();
            }
        }
    }
    
    public bool IsSFXPlaying(string sfxName)
    {
        AudioSource audioSource = GetAudioSourcePlaying(sfxName);
        return audioSource != null && audioSource.isPlaying;
    }

    public bool IsSFXPlaying(string sfxName, AudioSource specificSource)
    {
        return specificSource != null && specificSource.isPlaying && specificSource.clip != null && specificSource.clip.name == sfxName;
    }

    public string GetCurrentPlayingSFX()
    {
        foreach (AudioSource source in _audioSource)
        {
            if (source.isPlaying && source.clip != null)
            {
                return source.clip.name;
            }
        }
        return null;
    }

    private IEnumerator FadeOutSFXCoroutine(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;
        fadingCoroutines[audioSource] = true;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // reset volume for next time
        fadingCoroutines[audioSource] = false;
    }

    
}
