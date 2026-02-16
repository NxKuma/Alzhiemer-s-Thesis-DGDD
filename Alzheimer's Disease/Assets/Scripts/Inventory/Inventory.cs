using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
    private List<Item> _items;
    public event System.Action<Item> ItemAdded; // subscribers receive the added item
    public event System.Action<Item> ItemDropped; // subscribers receive the removed item
    public event System.Action<Item> ItemComplete; // subscribers receive the completed item
    public Inventory()
    {
        _items = new List<Item>();
    }

    public void Inventory_AddItem(Item item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            Debug.Log("Added: " + item.GetItemName());
            ItemAdded?.Invoke(item); // notify subscribers
            Debug.Log("Inventory now has added an item.");
        } else
        {
            Debug.Log("Added: " + item.GetItemName());
            ItemAdded?.Invoke(item); // notify subscribers
            Debug.Log("Inventory now has added an item.");
        }
    }

    //This is to remove the item permanently if it is used
    public void Inventory_RemoveItem(Item item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
        
            Debug.Log("Removed: " + item.GetItemName());
        }
    }

    //This is to remove the item from the inventory
    public void Inventory_DropItem(Item item)
    {   
        if (_items.Contains(item))
        {
            ItemDropped?.Invoke(item);
            Debug.Log("Dropped: " + item.GetItemName());
        }
    }

     //This is to remove the item from the inventory
    public void Inventory_CompleteItem(Item item)
    {   
        if (_items.Contains(item))
        {
            ItemComplete?.Invoke(item);
            Debug.Log("Completed: " + item.GetItemName());
        }
    }


    public List<Item> GetItemList()
    {
        return _items;
    }

}