using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleCAnvasScript : MonoBehaviour
{
    [SerializeField] private GameObject _puzzlePiecePrefab;
    private Transform _puzzleAreaParent;
    private CanvasGroup _canvasGroup;
    private bool _hasRandomized = false;
    private DialogueManager _dialogueManager;
    private List<GameObject> _puzzlePiecesAvailable = new List<GameObject>();
    private Inventory _puzzleInventory;
    
    private void Awake() {
        
        _puzzleAreaParent = this.transform.GetChild(1);
        _canvasGroup = this.GetComponent<CanvasGroup>();
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
            GameObject puzzlePiece = Instantiate(_puzzlePiecePrefab, _puzzleAreaParent);
            puzzlePiece.GetComponent<RectTransform>().sizeDelta = new Vector2(60,60);
            Image img = puzzlePiece.GetComponent<Image>();
            img.sprite = newItem.GetItemSprite();
            _puzzlePiecesAvailable.Add(puzzlePiece);
        }
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
        if (!_hasRandomized)
        {
            RandomizeChildOrder(_puzzleAreaParent);
            _hasRandomized = true;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        if(Cursor.lockState != CursorLockMode.None){  
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        _hasRandomized = false;
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
}

