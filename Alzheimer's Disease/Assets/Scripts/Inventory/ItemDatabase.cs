using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemState
{
    public Item item;            // reference to the Item ScriptableObject
    public ItemStatus status;   // runtime status
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items; // reference all your items here (for designers)

    private Dictionary<Item, ItemState> _runtimeStates;

    public void Init()
    {
        _runtimeStates = new Dictionary<Item, ItemState>();
        foreach (var item in items)
        {
            _runtimeStates[item] = new ItemState
            {
                item = item,
                status = ItemStatus.Hidden // default state
            };
        }
    }

    public ItemStatus GetStatus(Item item)
    {
        if (_runtimeStates == null) Init();
        return _runtimeStates[item].status;
    }

    public void SetStatus(Item item, ItemStatus newStatus)
    {
        if (_runtimeStates == null) Init();
        _runtimeStates[item].status = newStatus;
    }
}
