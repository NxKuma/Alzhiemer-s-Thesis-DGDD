using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _visibleInventory;
    [SerializeField] private GameObject _puzzleCountArea;
    private TextMeshProUGUI[] _puzzleCountTexts;
    private Texture2D[] _inventorySlots;
    private int[] _puzzleCounts;
    private Inventory _internalInventory;
    private int _inventorySlotCount;
    void Awake()
    {
        _inventorySlotCount = _visibleInventory.transform.childCount;
        _inventorySlots = new Texture2D[_inventorySlotCount];
        GameObject puzzleCountObj = _puzzleCountArea;

        _puzzleCountTexts = new TextMeshProUGUI[puzzleCountObj.transform.childCount];
        _puzzleCounts = new int[puzzleCountObj.transform.childCount];

        for (int i = 0; i < puzzleCountObj.transform.childCount; i++)
        {
            _puzzleCountTexts[i] = puzzleCountObj.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            _puzzleCounts[i] = 0;
        }

        for (int i = 0; i < _inventorySlotCount; i++)
        {
            Image img = _visibleInventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Color c = img.color;
            c.a = 0f;
            img.color = c;
            _inventorySlots[i] = img.sprite != null ? img.sprite.texture : null;
        }

        DontDestroyOnLoad(this.gameObject); 

        
    }

    void Start()
    {  
        if (TriggerHandler.Instance != null && TriggerHandler.Instance.PlayerInventory != null)
        {
            Debug.Log("Subscribing to inventory events in InventoryManager.");
            _internalInventory = TriggerHandler.Instance.PlayerInventory;
            _internalInventory.ItemAdded += AddToInventoryUI;
            _internalInventory.ItemDropped += RemoveFromInventoryUI;
        }
    }
    private void AddToInventoryUI(Item newItem)
    {
        if (newItem.GetItemtype() != Item.eItemType.JigsawPuzzle)
        {
            for (int i = 0; i < _inventorySlotCount; i++)
            {
                Image img = _visibleInventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                if (_inventorySlots[i] != null && img.color.a == 0f)
                {
                    Sprite itemSprite = newItem.GetItemSprite()[0];
                    _inventorySlots[i] = itemSprite.texture;
                    img.sprite = itemSprite;
                    Color c = img.color;
                    c.a = 1f;
                    img.color = c;
                    break;
                }
            }
        }
        
        for (int i = 0; i < _puzzleCountTexts.Length; i++)
        {
            Debug.Log(_puzzleCounts[i]);
            if (newItem.GetItemtype() == Item.eItemType.JigsawPuzzle)
            {
                if (newItem.GetItemName().Contains("Wife"))
                {   
                    _puzzleCounts[0] = int.Parse(_puzzleCountTexts[0].text.Substring(0,1));
                    _puzzleCounts[0]++;
                    _puzzleCountTexts[0].text = _puzzleCounts[0].ToString() + "/9";
                    break;
                }
                else if (newItem.GetItemName().Contains("Son")) 
                {
                    _puzzleCounts[1] = int.Parse(_puzzleCountTexts[1].text.Substring(0,1));
                    _puzzleCounts[1]++;
                    _puzzleCountTexts[1].text = _puzzleCounts[1].ToString() + "/9";
                    break;
                }
                else if (newItem.GetItemName().Contains("Daughter-in-Law")) 
                {
                    _puzzleCounts[2] = int.Parse(_puzzleCountTexts[2].text.Substring(0,1));
                    _puzzleCounts[2]++;
                    _puzzleCountTexts[2].text = _puzzleCounts[2].ToString() + "/9";
                    break;
                }
            }
        }
    }

    private void RemoveFromInventoryUI(Item removedItem)
    {
        for (int i = 1; i < _inventorySlotCount; i++)
        {
            Image img = _visibleInventory.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            if (_inventorySlots[i] == removedItem.GetItemSprite()[0].texture)
            {
                // _inventorySlots[i] = null;
                img.sprite = null;
                Color c = img.color;
                c.a = 0f;
                img.color = c;
                break;
            }
        }

        for (int i = 0; i < _puzzleCountTexts.Length; i++)
        {
            if (removedItem.GetItemtype() == Item.eItemType.JigsawPuzzle)
            {
                if (removedItem.GetItemName().Contains("Wife"))
                {   
                    _puzzleCounts[0] = int.Parse(_puzzleCountTexts[0].text.Substring(0,1));
                    _puzzleCounts[0]--;
                    _puzzleCountTexts[0].text = _puzzleCounts[0].ToString() + "/9";
                }
                else if (removedItem.GetItemName().Contains("Son")) 
                {
                    _puzzleCounts[1] = int.Parse(_puzzleCountTexts[1].text.Substring(0,1));
                    _puzzleCounts[1]--;
                    _puzzleCountTexts[1].text = _puzzleCounts[1].ToString() + "/9";
                }
                else if (removedItem.GetItemName().Contains("DIL")) 
                {
                    _puzzleCounts[2] = int.Parse(_puzzleCountTexts[2].text.Substring(0,1));
                    _puzzleCounts[2]--;
                    _puzzleCountTexts[2].text = _puzzleCounts[2].ToString() + "/9";
                }
            }
        }
    }
}
