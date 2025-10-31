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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _ran = Random.value;
    }

    public void PlayerInRoom(TriggerAreaScript area)
    {
        _currentArea = area;
        foreach (Item item in PlayerInventory.GetItemList())
        {
            Debug.Log(item);
             // Only process Main Quest items
            if (item.GetItemtype() != Item.eItemType.MainQuestItem)
                continue;

            // Check current status of the item
            ItemStatus status = TriggerAreaScript.GetItemStatus(item);
            Debug.Log(_ran);

            // if (status == ItemStatus.Dropped || status == ItemStatus.Spawned)
            // {
            //     Debug.Log($"{item.GetItemName()} is already {status}, skipping in {area.GetAreaName()}.");
            //     continue;
            // }

            // Roll chance to drop/spawn
            if (_ran < 0.5f && status == ItemStatus.Hidden)
            {
                area.DropItem(item);
                PlayerInventory.DropItem(item);
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
