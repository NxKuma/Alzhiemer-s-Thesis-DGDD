using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public static TriggerHandler Instance { get; private set; }
    public Inventory PlayerInventory { get; private set; }

    [SerializeField] private Transform _playerBounds;
    [SerializeField] private TriggerAreaScript[] _triggerAreas;

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

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerInRoom(TriggerAreaScript area)
    {
        _currentArea = area;
        foreach (var item in PlayerInventory.GetItemList())
        {
             // Only process Main Quest items
            if (item.GetItemtype() != Item.eItemType.MainQuestItem)
                continue;

            // Check current status of the item
            var status = TriggerAreaScript.GetItemStatus(item);

            if (status == ItemStatus.Dropped || status == ItemStatus.Spawned)
            {
                Debug.Log($"{item.GetItemName()} is already {status}, skipping in {area.GetAreaName()}.");
                continue;
            }

            // Roll chance to drop/spawn
            if (Random.value < 0.5f)
                area.DropItem(item);
            else
                area.SpawnItem(item);
            }
    }


    public void DropItem(Item item)
    {
        Debug.Log($"Dropping {item.GetItemName()} in {_currentArea.GetAreaName()}");
        _currentArea.DropItem(item);
        // TODO: Instantiate the prefab, mark state, etc.
    }

    public void SpawnItem(Item item)
    {
        Debug.Log($"Spawning {item.GetItemName()} in {_currentArea.GetAreaName()}");
        _currentArea.SpawnItem(item);
        // TODO: Instantiate the prefab, mark state, etc.
    }   
}
