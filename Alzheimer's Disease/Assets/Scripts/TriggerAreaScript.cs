using UnityEngine;
using System.Collections.Generic;

public class TriggerAreaScript : MonoBehaviour
{
    private enum eItemStatus { Dropped, Hidden, Spawned }
    Dictionary<Item, eItemStatus> _itemList = new Dictionary<Item, eItemStatus>();
    [SerializeField] private string _areaName;
    private static bool _hasPlayer = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DetectPlayer()
    {
        Debug.Log("Player Entered: " + _areaName);
        _hasPlayer = true;
    }
}
