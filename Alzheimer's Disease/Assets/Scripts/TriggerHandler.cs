using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField] private Transform _playerBounds;
    [SerializeField] private TriggerAreaScript[] _triggerAreas;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerInRoom()
    {

    }

}
