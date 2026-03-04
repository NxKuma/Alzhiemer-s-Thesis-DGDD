using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public static TriggerHandler Instance { get; private set; }
    public Inventory PlayerInventory { get; private set; }

    [SerializeField] private Transform _playerBounds;
    [SerializeField] private TriggerAreaScript[] _triggerAreas;
    [SerializeField, Range(0f, 1f)] private float _dropChance = 0.35f;
    [SerializeField, Range(0f, 1f)] private float _spawnChance = 0.90f;

    private TriggerAreaScript _currentArea;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        PlayerInventory = new Inventory();
        PlayerInventory.ItemComplete += item => TriggerAreaScript.SetItemStatus(item, ItemStatus.Complete);
    }   


    public void PlayerInRoom(TriggerAreaScript area)
    {
        _currentArea = area;

        foreach (Item item in PlayerInventory.GetItemList())
        {
             // Only process Main Quest items
            if (item.GetItemtype() != Item.eItemType.MainQuestItem)
                continue;

            // Check current status of the item
            ItemStatus status = TriggerAreaScript.GetItemStatus(item);

            // Drop only hidden items
            if (status == ItemStatus.Hidden && Random.value < _dropChance)
            {
                area.DropItem(item);
                PlayerInventory.Inventory_DropItem(item);
                Debug.Log($"Dropped {item.GetItemName()} in {area.GetAreaName()}.");

                // Refresh status after drop transition
                status = TriggerAreaScript.GetItemStatus(item);
            }

            // Spawn only dropped items, and only log success when spawn actually happens
            if (status == ItemStatus.Dropped && Random.value < _spawnChance)
            {
                bool spawned = area.TrySpawnItem(item);
                if (spawned)
                    Debug.Log($"Spawned {item.GetItemName()} in {area.GetAreaName()}.");
                else
                    Debug.Log($"Spawn attempt failed for {item.GetItemName()} in {area.GetAreaName()}.");
            }
        }
    }

    public TriggerAreaScript GetCurrentArea() => _currentArea;
}
