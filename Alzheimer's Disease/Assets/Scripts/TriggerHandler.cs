using UnityEngine;
using System.Collections.Generic;

public class TriggerHandler : MonoBehaviour
{
    public static TriggerHandler Instance { get; private set; }
    public Inventory PlayerInventory { get; private set; }

    [SerializeField] private Transform _playerBounds;
    [SerializeField] private TriggerAreaScript[] _triggerAreas;
    [SerializeField, Range(0f, 1f)] private float _dropChance = 0.45f;
    [SerializeField, Range(0f, 1f)] private float _spawnChance = 0.90f;
    [SerializeField, Min(1)] private int _forceSpawnAfterFailedChecks = 3;

    private TriggerAreaScript _currentArea;
    private readonly Dictionary<Item, int> _spawnMissStreak = new Dictionary<Item, int>();

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
            else if (status == ItemStatus.Dropped)
            {
                int missStreak = GetSpawnMissStreak(item);
                bool forceSpawn = missStreak >= _forceSpawnAfterFailedChecks;
                bool rolledSpawn = Random.value < _spawnChance;

                if (!forceSpawn && !rolledSpawn)
                {
                    IncrementSpawnMissStreak(item);
                    Debug.Log($"Spawn roll missed for {item.GetItemName()} in {area.GetAreaName()} ({GetSpawnMissStreak(item)}/{_forceSpawnAfterFailedChecks} before force spawn).");
                    continue;
                }

                // Spawn dropped items and force spawn after enough missed checks.
                bool spawned = area.TrySpawnItem(item);
                if (spawned)
                {
                    ResetSpawnMissStreak(item);
                    Debug.Log($"Spawned {item.GetItemName()} in {area.GetAreaName()}.");
                }
                else
                {
                    IncrementSpawnMissStreak(item);
                    area.TrySpawnItem(item);
                    Debug.Log($"Spawn attempt # {_spawnMissStreak[item]} failed for {item.GetItemName()} in {area.GetAreaName()}.");
                }
            }
            else
            {
                // Item is not waiting to spawn, so clear any stale miss streak.
                ResetSpawnMissStreak(item);
            }
        }
    }

    public TriggerAreaScript GetCurrentArea() => _currentArea;

    private int GetSpawnMissStreak(Item item)
    {
        if (item == null)
        {
            return 0;
        }

        if (_spawnMissStreak.TryGetValue(item, out int count))
        {
            return count;
        }

        return 0;
    }

    private void IncrementSpawnMissStreak(Item item)
    {
        if (item == null)
        {
            return;
        }

        _spawnMissStreak[item] = GetSpawnMissStreak(item) + 1;
    }

    private void ResetSpawnMissStreak(Item item)
    {
        if (item == null)
        {
            return;
        }

        _spawnMissStreak.Remove(item);
    }
}
