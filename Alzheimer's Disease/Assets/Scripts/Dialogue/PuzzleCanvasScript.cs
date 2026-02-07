using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleCAnvasScript : MonoBehaviour
{
    public static PuzzleCAnvasScript Instance { get; private set; }
    [SerializeField] private GameObject _puzzlePiecePrefab;
    private List<GameObject> _puzzlePiecesAvailable = new List<GameObject>();
    private List<GameObject> _puzzlePiecesList = new List<GameObject>();
    private Transform _puzzleAreaParent;
    private Transform _puzzleSlotsParent;
    private Transform _storageParent;
    private CanvasGroup _canvasGroup;
    private DialogueManager _dialogueManager;
    private CanvasManager _canvasManager;
    private Inventory _puzzleInventory;
    private string _nPCName;
    private bool _hasRandomized = false;
    private bool _isVisible = false;
    // Saved layouts per NPC: list of (slotName -> pieceName)
    private class SlotSave { public string slotName; public string pieceName; }
    private Dictionary<string, List<SlotSave>> _savedLayouts = new Dictionary<string, List<SlotSave>>();
    
    private void Awake() {
        
        _puzzleAreaParent = this.transform.GetChild(1);
        _puzzleSlotsParent = this.transform.GetChild(0).GetChild(1);
        // create a hidden storage parent for pieces so they aren't detected when talking to other NPCs
        GameObject storage = new GameObject("_PuzzleStorage");
        storage.transform.SetParent(this.transform, false);
        _storageParent = storage.transform;
        _canvasGroup = this.GetComponent<CanvasGroup>();
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }   
    }

    private void Start() {
        if (TriggerHandler.Instance != null && TriggerHandler.Instance.PlayerInventory != null)
        {
            _puzzleInventory = TriggerHandler.Instance.PlayerInventory;
            _puzzleInventory.ItemAdded += OnPuzzleItemAdded;

        }
        if (DialogueManager.GetInstance() != null)
        {
            _dialogueManager = DialogueManager.GetInstance();
        }

        Hide();
    }

    void Update() {
        if (_dialogueManager.DialogueIsPlaying)
        {
            Show();
        }else{
            Hide();
        }
    }

    private void OnPuzzleItemAdded(Item newItem)
    {
        if (newItem.GetItemtype() == Item.eItemType.JigsawPuzzle)
        {
            PuzzlePiece puzzlePieceData = newItem as PuzzlePiece;
            // Instantiate under storage by default; we'll move pieces into the area when interacting with an NPC
            GameObject puzzlePiece = Instantiate(_puzzlePiecePrefab, _storageParent);
            puzzlePiece.name = puzzlePieceData.GetItemName();
            GameObject puzzleImage = puzzlePiece.transform.GetChild(0).gameObject;
            // Debug.Log("Adding puzzle piece to puzzle canvas: " + puzzleImage.name);
            Image img = puzzleImage.GetComponent<Image>();
            puzzleImage.GetComponent<RectTransform>().localPosition = new Vector3(puzzlePieceData.GetPuzzleX(), puzzlePieceData.GetPuzzleY(), 0);
            puzzleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(puzzlePieceData.GetPuzzleWidth(), puzzlePieceData.GetPuzzleHeight());
            img.sprite = puzzlePieceData.GetItemSprite();
            _puzzlePiecesList.Add(puzzlePiece);
        }
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
        // Move pieces that belong to this NPC into the visible puzzle area and restore any saved layout
        if(!_isVisible) MovePiecesForNPC(_nPCName);
        RestoreLayoutForNPC(_nPCName);
        if (!_hasRandomized)
        {
            RandomizeChildOrder(_puzzleAreaParent);
            _hasRandomized = true;
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _isVisible = true;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        if(Cursor.lockState != CursorLockMode.Confined){  
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // Save any layout for the current NPC and move visible pieces back to storage 
        SaveLayoutForNPC(_nPCName);
        MovePiecesToStorage();
        _hasRandomized = false;
        _isVisible = false;
    }

    private void FilterPieces(string npcName)
    {
        _puzzlePiecesAvailable.Clear();
        foreach (GameObject piece in _puzzlePiecesList)
        {
            if(piece.name.Contains(npcName.Split('_')[0]))
            {
                piece.SetActive(true);
            }
            else
            {
                piece.SetActive(false);
            }
        }
    }

    private void MovePiecesForNPC(string npcName)
    {
        foreach (GameObject piece in _puzzlePiecesList)
        {
            if (piece == null) continue;
            if (!string.IsNullOrEmpty(npcName) && (piece.name.Contains(npcName.Split('-')[0]) || piece.name.Contains(npcName) ) )
            {
                piece.transform.SetParent(_puzzleAreaParent);
                piece.SetActive(true);
            }
            else
            {
                piece.transform.SetParent(_storageParent);
                piece.SetActive(false);
            }
        }
    }

    private void MovePiecesToStorage()
    {
        foreach (GameObject piece in _puzzlePiecesList)
        {
            if (piece == null) continue;
            piece.transform.SetParent(_storageParent, false);
            piece.SetActive(false);
        }
    }

    private void SaveLayoutForNPC(string npcName)
    {
        if (string.IsNullOrEmpty(npcName)) return;
        List<SlotSave> saved = new List<SlotSave>();
        // find all PuzzleSlot components under the puzzle area
        PuzzleSlot[] slots = _puzzleAreaParent.GetComponentsInChildren<PuzzleSlot>(true);
        foreach (var slot in slots)
        {
            string slotName = slot.gameObject.name;
            string pieceName = null;
            if (slot.transform.childCount > 0)
            {
                pieceName = slot.transform.GetChild(0).gameObject.name;
            }
            saved.Add(new SlotSave { slotName = slotName, pieceName = pieceName });
        }
        _savedLayouts[npcName] = saved;
    }

    private void RestoreLayoutForNPC(string npcName)
    {
        if (string.IsNullOrEmpty(npcName)) return;
        if (!_savedLayouts.ContainsKey(npcName)) return;
        var saved = _savedLayouts[npcName];
        foreach (var s in saved)
        {
            if (string.IsNullOrEmpty(s.pieceName)) continue;
            // find slot transform under puzzle area
            Transform slot = _puzzleAreaParent.Find(s.slotName);
            if (slot == null) continue;
            // find piece by name in our list
            GameObject piece = _puzzlePiecesList.Find(p => p != null && p.name == s.pieceName);
            if (piece == null) continue;
            piece.transform.SetParent(slot, false);
            piece.SetActive(true);
            // reset local transform so it snaps into slot
            RectTransform rt = piece.GetComponent<RectTransform>();
            if (rt != null) { rt.anchoredPosition = Vector2.zero; rt.localRotation = Quaternion.identity; }
        }
    }

    private void RandomizeChildOrder(Transform parent)
    {
        // Create a list of all children
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        // Fisher-Yates shuffle
        for (int i = children.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Swap by changing sibling index
            children[i].SetSiblingIndex(randomIndex);
            children[randomIndex].SetSiblingIndex(i);
            // Swap in list too
            Transform temp = children[i];
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }
    }

    public void SetNPCName(string name)
    {
        _nPCName = name;
    }
}

