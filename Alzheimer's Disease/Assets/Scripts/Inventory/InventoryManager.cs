using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _visibleInventory;
    private Texture2D[] _inventorySlots;
    // private Image[] _visibleInventoryImages;
    private Inventory _internalInventory;
    private int _inventorySlotCount;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        _inventorySlotCount = _visibleInventory.transform.childCount;
        _inventorySlots = new Texture2D[_inventorySlotCount];

        for (int i = 0; i < _inventorySlotCount; i++)
        {
            Image img = _visibleInventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            
            Color c = img.color;
            c.a = 0f;
            img.color = c;
            _inventorySlots[i] = img.sprite != null ? img.sprite.texture : null;
        }

        DontDestroyOnLoad(this.gameObject); 

        if (TriggerHandler.Instance != null && TriggerHandler.Instance.PlayerInventory != null)
        {
            _internalInventory = TriggerHandler.Instance.PlayerInventory;
            _internalInventory.ItemAdded += AddToInventoryUI;
            _internalInventory.ItemDropped += RemoveFromInventoryUI;
        }
    }
    private void AddToInventoryUI(Item newItem)
    {
        for (int i = 0; i < _inventorySlotCount; i++)
        {
            Image img = _visibleInventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            if (_inventorySlots[i] != null && img.color.a == 0f)
            {
                Texture2D itemSprite = newItem.GetItemTexture();
                _inventorySlots[i] = itemSprite;
                Sprite sprite = Sprite.Create(itemSprite, new Rect(0, 0, itemSprite.width, itemSprite.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;
                Color c = img.color;
                c.a = 1f;
                img.color = c;
                break;
            }
        }
    }

    private void RemoveFromInventoryUI(Item removedItem)
    {
        for (int i = 0; i < _inventorySlotCount; i++)
        {
            Image img = _visibleInventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            if (_inventorySlots[i] == removedItem.GetItemTexture())
            {
                _inventorySlots[i] = null;
                img.sprite = null;
                Color c = img.color;
                c.a = 0f;
                img.color = c;
                break;
            }
        }
    }
}
