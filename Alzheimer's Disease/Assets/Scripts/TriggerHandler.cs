using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public static TriggerHandler Instance { get; private set; }
    public Inventory PlayerInventory { get; private set; }

    [SerializeField] private Transform _playerBounds;
    [SerializeField] private TriggerAreaScript[] _triggerAreas;

    private TriggerAreaScript _currentArea;
    private float _ran;

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

    }

    public void PlayerInRoom(TriggerAreaScript area)
    {
        _currentArea = area;
        _ran = Random.value;

        foreach (Item item in PlayerInventory.GetItemList())
        {
             // Only process Main Quest items
            if (item.GetItemtype() != Item.eItemType.MainQuestItem)
                continue;

            // Check current status of the item
            ItemStatus status = TriggerAreaScript.GetItemStatus(item);

            // Roll chance to drop/spawn
            if (_ran < 0.5f && status == ItemStatus.Hidden)
            {
                area.DropItem(item);
                PlayerInventory.Inventory_DropItem(item);
                Debug.Log("Dropping...");
            }else
                Debug.Log($"{item.GetItemName()} is already {status}, skipping in {area.GetAreaName()}.");


            if (_ran >= 0.5f && status == ItemStatus.Dropped && status != ItemStatus.Spawned)
            {
                area.SpawnItem(item);
                Debug.Log("Spawning...");
            }
        }
    }
}
