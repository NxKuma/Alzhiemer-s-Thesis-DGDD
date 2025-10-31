using UnityEngine;
using System.Collections.Generic;

public class TriggerAreaScript : MonoBehaviour
{
    private static Dictionary<Item, ItemStatus> _itemList = new Dictionary<Item, ItemStatus>();
    [SerializeField] private GameObject _itemSpawnPrefab; 
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private string _areaName;
    [SerializeField] private float _spawnClearRadius = 0.18f;
    private static bool _hasPlayer = false;
    private Collider _areaCollider;

    void Awake()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    private Vector3 RandomPointInBounds(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            Random.Range(b.min.z, b.max.z)
        );
    }

    public static ItemStatus GetItemStatus(Item item)
    {
        if (_itemList.TryGetValue(item, out ItemStatus status))
            return status;

        return ItemStatus.Hidden;
    }

    private static void SetItemStatus(Item item, ItemStatus status)
    {
        _itemList[item] = status;
    }

    public bool SpawnItemInArea(Item item)
    {
        if (_areaCollider == null) _areaCollider = GetComponent<Collider>();

        Bounds b = _areaCollider.bounds;

        // sample in bounds
        Vector3 candidate = RandomPointInBounds(b);

        // ensure the sampled point is actually inside the collider volume (ClosestPoint returns the closest point on collider surface)
        Vector3 closest = _areaCollider.ClosestPoint(candidate);

        // If closest != candidate (distance > small epsilon) then candidate lies outside the collider volume
        while (Vector3.Distance(closest, candidate) > 0.05f)
             candidate = RandomPointInBounds(b); // not inside; try again

        // Raycast down from above to find ground surface
        Vector3 rayStart = candidate + Vector3.up * (b.extents.y + 2f);
        int mask = LayerMask.GetMask("Ground");
        float rayDistance = b.extents.y + 4f;
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayDistance, mask))
        {
            Vector3 spawnPos = hit.point;
            return InstantiateItemAt(item, spawnPos);
        }
        else
        {
            return InstantiateItemAt(item, candidate);
        }
    }

    private bool InstantiateItemAt(Item item, Vector3 pos)
    {
        GameObject itemInst = Instantiate(_itemSpawnPrefab, pos, Quaternion.identity);
        // parent to area for organization
        itemInst.transform.SetParent(this.transform, true);

        // try to initialize via ItemScript if present
        ItemScript itemScript = itemInst.GetComponent<ItemScript>();
        if (itemScript != null)
        {
            itemScript.Initialize(item); // you added this earlier
        }
        else
        {
            // fallback: try to set mesh/material directly
            MeshFilter mf = itemInst.GetComponent<MeshFilter>();
            if (mf != null && item.GetItemMesh() != null) mf.mesh = item.GetItemMesh();
            Renderer rend = itemInst.GetComponent<Renderer>();
            if (rend != null && item.GetItemMaterial() != null) rend.material = item.GetItemMaterial();
        }

        // update global status
        SetItemStatus(item, ItemStatus.Spawned);

        return true;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void DetectPlayer()
    {
        Debug.Log("Player Entered: " + _areaName);
        _hasPlayer = true;
        if(TriggerHandler.Instance != null) TriggerHandler.Instance.PlayerInRoom(this);
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
        
    }

    public void SpawnItem(Item item)
    {

        if (GetItemStatus(item) == ItemStatus.Spawned)
        {
            Debug.Log($"{item.GetItemName()} is already {GetItemStatus(item)}, skipping spawn.");
            return;
        }

        Debug.Log($"Spawned {item.GetItemName()} in {_areaName}");
        SetItemStatus(item, ItemStatus.Spawned);

        // TODO: Instantiate prefab
        SpawnItemInArea(item);
    }

    public void CheckItem(Item item)
    {
        ItemStatus status = _itemDatabase.GetStatus(item);
        Debug.Log($"{item.GetItemName()} is currently {status}");
    }

    //Getters
    public ItemDatabase GetItemDatabase() => _itemDatabase;
    public string GetAreaName() => _areaName;
}
