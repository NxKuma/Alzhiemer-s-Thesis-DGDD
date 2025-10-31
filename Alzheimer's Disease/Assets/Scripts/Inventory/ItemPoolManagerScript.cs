using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemPoolManagerScript : MonoBehaviour
{
    public static ItemPoolManagerScript Instance { get; private set; }
    [SerializeField] private ItemScript[] _itemPool;
    
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

    public static void UpdatePool()
    {
        foreach (ItemScript iS in _itemPool)
        {
            _spawnPool[iS] = iS.gameObject.activeSelf;
        }
    }

    public bool ItemSpawnable(ItemScript item) => _spawnPool[item];

    
    
}
