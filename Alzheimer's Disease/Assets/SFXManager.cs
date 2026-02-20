using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(string sfxName, bool islooping = false)
    {
        foreach (AudioClip clip in _audioClips)
        {
            if (clip.name == sfxName)
            {
                _audioSource.clip = clip;
                if (islooping)
                {
                    _audioSource.loop = true;
                    if (!_audioSource.isPlaying)
                    {
                        Debug.Log($"Playing looping SFX: {sfxName}");
                        _audioSource.Play();
                    }
                    return;
                }
                else
                {
                    Debug.Log($"Playing one-shot SFX: {sfxName}");
                    _audioSource.loop = false;
                    _audioSource.PlayOneShot(clip);
                    return;
                }
            }
        }
        Debug.LogWarning($"SFXManager: No audio clip found with name {sfxName}");
    }

    public void StopSFX(string sfxName)
    {
        if (_audioSource.clip != null && _audioSource.clip.name == sfxName)
        {
            Debug.Log($"Stopping SFX: {sfxName}");
            _audioSource.Stop();
        }
    }
}
