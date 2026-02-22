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
        intenseMusic.Stop();
    }
}
