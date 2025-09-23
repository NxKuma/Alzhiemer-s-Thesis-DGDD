using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
    private List<Item> _items;

    public Inventory()
    {
        _items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (!_items.Contains(item)) // avoid duplicates if you want
        {
            _items.Add(item);
            Debug.Log("Added: " + item.GetItemName());
        }
    }

    public void RemoveItem(Item item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            Debug.Log("Removed: " + item.GetItemName());
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
