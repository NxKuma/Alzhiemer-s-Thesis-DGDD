using UnityEngine;
using System.Collections.Generic;

public class TriggerAreaScript : MonoBehaviour
{
    // private enum eItemStatus { Dropped, Hidden, Spawned }
    // private static Dictionary<Item, eItemStatus> _itemList = new Dictionary<Item, eItemStatus>();
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private string _areaName;
    private static bool _hasPlayer = false;

    void Awake() {
        this.GetComponent<Renderer>().enabled = false;  
    }

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

    public string GetAreaName()
    {
        return _areaName;
    }

    public void DropItem(Item item)
    {
        _itemDatabase.SetStatus(item, eItemStatus.Dropped);
        Debug.Log($"{item.GetItemName()} dropped in {_areaName}");
    }

    public void SpawnItem(Item item)
    {
        _itemDatabase.SetStatus(item, eItemStatus.Spawned);
        Debug.Log($"{item.GetItemName()} spawned in {_areaName}");
    }

    public void CheckItem(Item item)
    {
        var status = _itemDatabase.GetStatus(item);
        Debug.Log($"{item.GetItemName()} is currently {status}");
    }

}
