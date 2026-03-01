using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemPoolManagerScript : MonoBehaviour
{
    public static ItemPoolManagerScript Instance { get; private set; }
    [SerializeField] private ItemScript[] _itemPool;
    private Inventory _proxyInventory;
    private static Dictionary<ItemScript, bool> _spawnPool = new Dictionary<ItemScript, bool>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);


        foreach (ItemScript iS in _itemPool)
        {
            _spawnPool[iS] = iS.gameObject.activeSelf;
        }
    }

    void Start()
    {
        _proxyInventory = TriggerHandler.Instance.PlayerInventory;
        _proxyInventory.ItemAdded += UpdatePool;
        _proxyInventory.ItemDropped += SetItemSpawnable;
    }

    public void UpdatePool(Item item)
    {
        // if (item is Puzz) return;
        if(item.GetItemtype() == Item.eItemType.JigsawPuzzle) return;

        foreach (ItemScript iS in _itemPool)
        {
            _spawnPool[iS] = iS.gameObject.activeSelf;
        }
        // mark item as hidden in global area tracking when added to inventory
        TriggerAreaScript.SetItemStatus(item, ItemStatus.Hidden);
    }
    
    public void SetItemSpawnable(Item item)
    {
        if(item.GetItemtype() == Item.eItemType.JigsawPuzzle) return;

        foreach (ItemScript iS in _itemPool)
        {
            if (iS.GetItemResource() == item)
            {   
                Debug.Log($"Setting {item.GetItemName()} spawnable in pool");
                _spawnPool[iS] = true;
                return;
            }
        }
    }

    public bool ItemSpawnable(ItemScript item) => _spawnPool[item];
    public ItemScript[] GetItemPool() => _itemPool;
    
}
