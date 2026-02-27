using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TriggerAreaScript : MonoBehaviour
{
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private string _areaName;
    private static Dictionary<Item, ItemStatus> _itemList = new Dictionary<Item, ItemStatus>();
    private static bool _hasPlayer = false;
    private Collider _areaCollider;
    private SFXManager sFXManager;

    //TESTING ONLY - TO BE REMOVED LATER
    private Music music;
    private event System.Action<string> PlayerEntered;

    void Awake()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    void Start()
    {
        sFXManager = SFXManager.Instance;
        music = Music.Instance;
    } 

    //DON'T DELETE THIS: Used for triggering spawn and drop areas
    public void DetectPlayer()
    {
        Debug.Log("Player Entered: " + _areaName);
        if(_areaName == "LivingRoom" && music != null)
        {
            music.SwapTrack();
        }
        _hasPlayer = true;
        if(TriggerHandler.Instance != null) TriggerHandler.Instance.PlayerInRoom(this);
    }

    private Vector3 RandomPointInBounds(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            Random.Range(b.min.z, b.max.z)  
        );
    }

    public static void SetItemStatus(Item item, ItemStatus status)
    {
        _itemList[item] = status;
    }

    private bool SpawnItemInArea(Item item)
    {
        // Debug.Log("Attempting to spawn " + item.GetItemName() + " in " + _areaName);
        
        // if (_areaCollider == null) _areaCollider = GetComponent<Collider>();


        // Bounds b = _areaCollider.bounds;

        // // sample in bounds
        // Vector3 candidate = RandomPointInBounds(b);

        // // ensure the sampled point is actually inside the collider volume (ClosestPoint returns the closest point on collider surface)
        // Vector3 closest = _areaCollider.ClosestPoint(candidate);

        // // If closest != candidate (distance > small epsilon) then candidate lies outside the collider volume
        // while (Vector3.Distance(closest, candidate) > 0.05f)
        //      candidate = RandomPointInBounds(b); // not inside; try again

        // // Raycast down from above to find ground surface
        // Vector3 rayStart = candidate + Vector3.up * (b.extents.y + 2f);
        // int mask = LayerMask.GetMask("Ground");
        // float rayDistance = b.extents.y + 4f;
        // if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayDistance, mask))
        // {
        //     Vector3 spawnPos = hit.point;
        //     return InstantiateItemAt(item, spawnPos);
        // }
        // else
        // {
        //     return InstantiateItemAt(item, candidate);
        // }

        int randomIndex = Random.Range(0, _spawnPoints.Length); 
        Vector3 spawnPos = _spawnPoints[randomIndex].position;
        return InstantiateItemAt(item, spawnPos);

    }

    private bool InstantiateItemAt(Item item, Vector3 pos)
    {
        foreach (ItemScript iS in ItemPoolManagerScript.Instance.GetItemPool())
        {
            Debug.Log($"Checking item pool for {item.GetItemName()}");
            if (iS.GetItemResource() == item)
            {   
                Debug.Log($"Item is found");
                if (!ItemPoolManagerScript.Instance.ItemSpawnable(iS))
                {
                    Debug.Log($"{item.GetItemName()} is not spawnable right now.");
                    return false;
                }
                else
                {
                    iS.gameObject.transform.position = pos;
                    iS.gameObject.SetActive(true);
                    SetItemStatus(item, ItemStatus.Spawned);
                    iS.TweenShadowThickness(0f, 8f);
                    Debug.Log($"Spawned {item.GetItemName()} in {_areaName}");

                    return true;
                }
            }
        }
        return false;
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
        sFXManager.PlaySFX("drop_item");
    }

    public void SpawnItem(Item item)
    {
        if (GetItemStatus(item) == ItemStatus.Spawned)
        {
            Debug.Log($"{item.GetItemName()} is already {GetItemStatus(item)}, skipping spawn.");
            return;
        }
        SpawnItemInArea(item);
    }

    #region GETTERS
    //Getters
    public ItemDatabase GetItemDatabase() => _itemDatabase;
    public string GetAreaName() => _areaName;

    public static ItemStatus GetItemStatus(Item item)
    {
        if (_itemList.TryGetValue(item, out ItemStatus status))
            return status;

        return ItemStatus.Hidden;
    }
    #endregion
}
