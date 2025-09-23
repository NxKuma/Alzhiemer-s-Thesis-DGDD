using UnityEngine;
using System.Collections.Generic;

public class TriggerAreaScript : MonoBehaviour
{
    private static Dictionary<Item, ItemStatus> _itemList = new Dictionary<Item, ItemStatus>();
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private string _areaName;
    private static bool _hasPlayer = false;

    void Awake()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    public static ItemStatus GetItemStatus(Item item)
    {
        if (_itemList.TryGetValue(item, out var status))
            return status;

        return ItemStatus.Hidden;
    }

    private static void SetItemStatus(Item item, ItemStatus status)
    {
        _itemList[item] = status;
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


    public void DropItem(Item item)
    {
        if (GetItemStatus(item) == ItemStatus.Dropped || GetItemStatus(item) == ItemStatus.Spawned)
        {
            Debug.Log($"{item.GetItemName()} is already {GetItemStatus(item)}, skipping drop.");
            return;
        }

        Debug.Log($"Dropped {item.GetItemName()} in {_areaName}");
        SetItemStatus(item, ItemStatus.Dropped);

        // TODO: Instantiate prefab
    }

    public void SpawnItem(Item item)
    {

        if (GetItemStatus(item) == ItemStatus.Spawned || GetItemStatus(item) == ItemStatus.Dropped)
        {
            Debug.Log($"{item.GetItemName()} is already {GetItemStatus(item)}, skipping spawn.");
            return;
        }

        Debug.Log($"Spawned {item.GetItemName()} in {_areaName}");
        SetItemStatus(item, ItemStatus.Spawned);

        // TODO: Instantiate prefab
    }

    public void CheckItem(Item item)
    {
        var status = _itemDatabase.GetStatus(item);
        Debug.Log($"{item.GetItemName()} is currently {status}");
    }

    //Getters
    public ItemDatabase GetItemDatabase() => _itemDatabase;
    public string GetAreaName() => _areaName;
}
