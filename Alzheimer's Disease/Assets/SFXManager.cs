using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource[] _audioSource;
    [SerializeField] private AudioClip[] _audioClips;

    void Awake()
    {
        Instance = this;
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
        foreach (AudioClip clip in _audioClips)
        {
            if (clip.name == sfxName)
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
        AudioSource _audioSource = GetAudioSourcePlaying(sfxName);
        if (_audioSource != null)
        {
            Debug.Log($"Stopping SFX: {sfxName}");
            _audioSource.Stop();
        }
    }
    
    public bool IsSFXPlaying(string sfxName)
    {
        AudioSource _audioSource = GetAudioSourcePlaying(sfxName);
        return _audioSource != null && _audioSource.isPlaying;
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
}
