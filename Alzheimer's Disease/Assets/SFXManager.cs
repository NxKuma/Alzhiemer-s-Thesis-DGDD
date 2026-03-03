using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Screen.SetResolution(1920, 1080, true);
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
        return null; // fallback to first audio source if all are busy
    } 

    private AudioSource GetAudioSourcePlaying(string sfxName)
    {
        foreach (AudioSource source in _audioSource)
        {
            if (source.isPlaying && source.clip != null && source.clip.name.Contains(sfxName))
            {
                return source;
            }
        }
        return null;
    }

    public async Task PlaySFX(string sfxName, bool islooping = false)
    {
        float pitcchVariation = Random.Range(0.925f, 1.075f);
        // Only allow one AudioSource to play a given SFX at a time.
        if (GetAudioSourcePlaying(sfxName) != null)
        {
            // Debug.Log($"SFX '{sfxName}' is already playing on another audio source. Skipping play request.");
            return;
        }

        AudioSource availAudio = GetAvailableAudioSource();
        if (availAudio == null)
        {
            // Debug.LogWarning($"SFXManager: No available audio source to play SFX '{sfxName}'. Skipping play request.");
            return;
        }

        availAudio.volume = 1f;
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
                        // Debug.Log($"Playing looping SFX: {sfxName}");
                        availAudio.Play();
                    }else
                    {
                        return;
                    }
                    return;
                }
                else
                {
                    // StopAllCoroutines(); // stop any ongoing fade coroutines to prevent volume conflicts
                    // Debug.Log($"Playing one-shot SFX: {sfxName}");
                    availAudio.loop = false;
                    availAudio.pitch = pitcchVariation;
                    availAudio.PlayOneShot(clip);
                    await Task.Delay((int)(clip.length * 1000)); // wait for clip to finish playing
                    availAudio.clip = null; // clear clip reference after playing one-shot to free up audio source for next use
                    return;
                }
            }
        }
    }

    public async void StopSFX(string sfxName)
    {
        AudioSource audioSource = GetAudioSourcePlaying(sfxName);
        if (audioSource != null)
        {
            if (audioSource.loop || !fadingCoroutines[audioSource])
            {
                Task fade = FadeOutSFXAsync(audioSource, 0.5f);
                await fade;
                ResetAudioSource(audioSource);

            }
            else if (!audioSource.loop)
            {
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
        return specificSource != null && specificSource.isPlaying && specificSource.clip != null && specificSource.clip.name.Contains(sfxName);
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

    private async Task FadeOutSFXAsync(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;
        fadingCoroutines[audioSource] = true;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            await Task.Yield();
        }
    }

    private void ResetAudioSource(AudioSource audioSource)
    {
        fadingCoroutines[audioSource] = false;
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.volume = 1f; // reset volume for next time
    }


}
