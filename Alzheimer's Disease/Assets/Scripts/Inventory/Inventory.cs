using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Inventory
{
    private List<Item> _items;
    public event System.Action<Item> ItemAdded; // subscribers receive the added item
    public event System.Action<Item> ItemDropped; // subscribers receive the removed item
    public Inventory()
    {
        _items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            Debug.Log("Added: " + item.GetItemName());
            ItemAdded?.Invoke(item); // notify subscribers
            Debug.Log("Inventory now has added an item.");
        }
    }

    //This is to remove the item permanently if it is used
    public void RemoveItem(Item item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);

            Debug.Log("Removed: " + item.GetItemName());
        }
    }

    //This is to remove the item from the inventory
    public void DropItem(Item item)
    {   
        if (_items.Contains(item))
        {
            ItemDropped?.Invoke(item);
            Debug.Log("Dropped: " + item.GetItemName());
        }
    }

    public bool HasItem(Item item)
    {
        return _items.Contains(item);
    }

    public List<Item> GetItemList()
    {
        return _items;
    }

}
