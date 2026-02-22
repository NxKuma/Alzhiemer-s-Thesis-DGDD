using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance = null;
    [SerializeField] private AudioSource calmMusic;
    [SerializeField] private AudioSource intenseMusic;

    public static Music Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {
        calmMusic.Play();
        intenseMusic.volume = 0f; // start intense music muted
        intenseMusic.Play();
    }

    public void SwapTrack()
    {
        StopAllCoroutines(); // stop any ongoing fade coroutines to prevent conflicts
        StartCoroutine(FadeTrack());
    }


    private IEnumerator FadeTrack(bool isCalmToIntense = true)
    {
        AudioSource trackToFade = isCalmToIntense ? calmMusic : intenseMusic;
        AudioSource trackToPlay = isCalmToIntense ? intenseMusic : calmMusic;
        float duration = 1.25f; // duration of fade in seconds
        float startVolume = isCalmToIntense? 0.2f : 0.95f;
        float targetVolume = isCalmToIntense? 0.95f : 0.2f;
        float elapsedTime = 0f;

        if ((isCalmToIntense && calmMusic.volume == 0f) || (!isCalmToIntense && intenseMusic.volume == 0f) )
        {
            //Don't fade out if the track isn't playing
            yield break;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            trackToFade.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            trackToPlay.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / duration);
            yield return null;
        }
    }

}
