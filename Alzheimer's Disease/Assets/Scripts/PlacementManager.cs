using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct PlacementInfo
{
    public Vector3 position;
    public Vector3 rotation;
    public Transform room;
    public bool isVisible;

}

public enum PlacementPhase
{
    Phase1 = 1,
    Phase2 = 2,
    Phase3 = 3
}

// Unity can't serialize Dictionary directly, so this stores keys/values in parallel lists
// and rebuilds a runtime Dictionary on load.
[Serializable]
public class PlacementDictionary : ISerializationCallbackReceiver
{
    [SerializeField] private List<GameObject> _gameObjects = new List<GameObject>();
    [SerializeField] private List<PlacementInfo> _placementValues = new List<PlacementInfo>();

    private Dictionary<GameObject, PlacementInfo> _dict;

    public void EnsureKeys(IEnumerable<GameObject> keys)
    {
        if (keys == null)
        {
            return;
        }

        // Keep lists aligned even if edited manually.
        while (_placementValues.Count < _gameObjects.Count)
        {
            _placementValues.Add(default);
        }
        while (_placementValues.Count > _gameObjects.Count)
        {
            _placementValues.RemoveAt(_placementValues.Count - 1);
        }

        bool changed = false;
        foreach (GameObject key in keys)
        {
            if (key == null)
            {
                continue;
            }

            if (_gameObjects.Contains(key))
            {
                continue;
            }

            _gameObjects.Add(key);
            _placementValues.Add(default);
            changed = true;
        }

        if (changed)
        {
            _dict = null;
        }
    }

    public bool TryGetValue(GameObject gameObject, out PlacementInfo info)
    {
        EnsureBuilt();
        return _dict.TryGetValue(gameObject, out info);
    }

    public Dictionary<GameObject, PlacementInfo> AsDictionary()
    {
        EnsureBuilt();
        return _dict;
    }

    public bool SetValue(GameObject gameObject, PlacementInfo info)
    {
        if (gameObject == null)
        {
            return false;
        }

        int index = _gameObjects.IndexOf(gameObject);
        if (index < 0)
        {
            _gameObjects.Add(gameObject);
            _placementValues.Add(info);
        }
        else
        {
            while (_placementValues.Count <= index)
            {
                _placementValues.Add(default);
            }

            _placementValues[index] = info;
        }

        EnsureBuilt();
        _dict[gameObject] = info;
        return true;
    }

    public void OnBeforeSerialize()
    {
        // Lists are edited in the Inspector.
    }

    public void OnAfterDeserialize()
    {
        _dict = null;
    }

    private void EnsureBuilt()
    {
        if (_dict != null)
        {
            return;
        }

        _dict = new Dictionary<GameObject, PlacementInfo>();

        int count = Mathf.Min(_gameObjects.Count, _placementValues.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject key = _gameObjects[i];
            if (key == null)
            {
                continue;
            }

            // If duplicates exist, last one wins.
            _dict[key] = _placementValues[i];
        }
    }
}

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private ItemPoolManagerScript _itemPoolManager;
    [SerializeField] private QuestManager _questManager;

    [Header("Placement Data (Serialized Dictionary)")]
    [Tooltip("Maps GameObject -> placement info (position + required gamePhase).")]
    [SerializeField] private PlacementDictionary _phase1Placements = new PlacementDictionary();
    [SerializeField] private PlacementDictionary _phase2Placements = new PlacementDictionary();
    [SerializeField] private PlacementDictionary _phase3Placements = new PlacementDictionary();

    [Header("Editor Placement Save")]
    [SerializeField] private PlacementPhase _editorTargetPhase = PlacementPhase.Phase1;

    private void OnValidate()
    {
        AutoPopulatePlacementKeys();
    }

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple PlacementManager instances detected. There should only be one PlacementManager in the scene.", this);
            Destroy(this);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dialogueManager = DialogueManager.GetInstance();
        _dialogueManager.GetDialogueVariables().GamePhaseChanged += OnGamePhaseChanged;
        _itemPoolManager = ItemPoolManagerScript.Instance;

        // Runtime safety: make sure keys exist even if OnValidate didn't run.
        AutoPopulatePlacementKeys();
        OnGamePhaseChanged(1);
    }

    private void OnGamePhaseChanged(int newGamePhaseInt)
    {
        PlacementPhase newPlacementPhase = (PlacementPhase)newGamePhaseInt;
        Debug.Log($"Game phase changed: {newPlacementPhase}. Updating placements.", this);

        PlacementDictionary targetPlacements = GetPlacementsForPhase(newPlacementPhase);
        if (targetPlacements == null)
        {
            Debug.LogWarning($"No placement dictionary configured for phase {newPlacementPhase}.", this);
            return;
        }

        Dictionary<GameObject, PlacementInfo> placements = targetPlacements.AsDictionary();
        if (placements == null || placements.Count == 0)
        {
            Debug.LogWarning($"No placement keys found for phase {newPlacementPhase}. Auto-populate first.", this);
            return;
        }

        foreach (KeyValuePair<GameObject, PlacementInfo> kvp in placements)
        {
            GameObject gameObject = kvp.Key;
            PlacementInfo info = kvp.Value;

            if (gameObject == null)
            {
                continue;
            }

            if (info.room != null)
            {
                gameObject.transform.SetParent(info.room);
            }
            gameObject.SetActive(info.isVisible);

            gameObject.transform.position = info.position;
            gameObject.transform.eulerAngles = info.rotation;

        }
    }

    private void AutoPopulatePlacementKeys()
    {
        // Resolve references in edit mode too.
        if (_dialogueManager == null)
        {
            #if UNITY_2023_1_OR_NEWER
            _dialogueManager = UnityEngine.Object.FindFirstObjectByType<DialogueManager>(FindObjectsInactive.Include);
            #else
            _dialogueManager = FindObjectOfType<DialogueManager>(true);
            #endif
        }
        if (_itemPoolManager == null)
        {
            #if UNITY_2023_1_OR_NEWER
            _itemPoolManager = UnityEngine.Object.FindFirstObjectByType<ItemPoolManagerScript>(FindObjectsInactive.Include);
            #else
            _itemPoolManager = FindObjectOfType<ItemPoolManagerScript>(true);
            #endif
        }

        List<GameObject> keysToEnsure = new List<GameObject>();

        // NPCs: include NPCScript objects that match NPC data configured in DialogueManager.
        HashSet<string> allowedNpcNames = null;
        if (_dialogueManager != null)
        {
            allowedNpcNames = new HashSet<string>(StringComparer.Ordinal);
            foreach (NPC npc in _dialogueManager.GetNpcData())
            {
                allowedNpcNames.Add(npc.GetNPCName());
            }
        }

        #if UNITY_2023_1_OR_NEWER
        NPCScript[] npcScripts = UnityEngine.Object.FindObjectsByType<NPCScript>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        #else
        NPCScript[] npcScripts = FindObjectsOfType<NPCScript>(true);
        #endif
        foreach (NPCScript npcScript in npcScripts)
        {
            if (npcScript == null)
            {
                continue;
            }

            if (allowedNpcNames != null && allowedNpcNames.Count > 0)
            {
                string npcName = npcScript.GetNPCName();
                if (!allowedNpcNames.Contains(npcName))
                {
                    continue;
                }
            }

            keysToEnsure.Add(npcScript.gameObject);
        }

        // Items: include every ItemScript that exists in the ItemPoolManagerScript pool.
        if (_itemPoolManager != null)
        {
            ItemScript[] itemPool = _itemPoolManager.GetItemPool();
            if (itemPool != null)
            {
                foreach (ItemScript itemScript in itemPool)
                {
                    if (itemScript == null)
                    {
                        continue;
                    }

                    keysToEnsure.Add(itemScript.gameObject);
                }
            }
        }

        _phase1Placements?.EnsureKeys(keysToEnsure);
        _phase2Placements?.EnsureKeys(keysToEnsure);
        _phase3Placements?.EnsureKeys(keysToEnsure);

        #if UNITY_EDITOR
        // Mark scene dirty when auto-populating in editor.
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }

    public PlacementPhase GetEditorTargetPhase()
    {
        return _editorTargetPhase;
    }

    public void SavePlacementsForEditorPhase()
    {
        SavePlacementsForPhase(_editorTargetPhase);
    }

    public void SavePlacementsForPhase(PlacementPhase phase)
    {
        AutoPopulatePlacementKeys();

        PlacementDictionary targetPlacements = GetPlacementsForPhase(phase);
        if (targetPlacements == null)
        {
            Debug.LogWarning($"No placement dictionary configured for phase {phase}.", this);
            return;
        }

        Dictionary<GameObject, PlacementInfo> placements = targetPlacements.AsDictionary();
        if (placements == null || placements.Count == 0)
        {
            Debug.LogWarning($"No placement keys found for phase {phase}. Auto-populate first.", this);
            return;
        }

        List<GameObject> keys = new List<GameObject>(placements.Keys);

        int savedCount = 0;
        foreach (GameObject gameObject in keys)
        {
            if (gameObject == null)
            {
                continue;
            }

            if (!placements.TryGetValue(gameObject, out PlacementInfo info))
            {
                continue;
            }

            Transform gameObjectTransform = gameObject.transform;
            info.position = gameObjectTransform.position;
            info.rotation = gameObjectTransform.eulerAngles;
            info.room = gameObjectTransform.parent;
            info.isVisible = gameObject.activeSelf;

            if (targetPlacements.SetValue(gameObject, info))
            {
                savedCount++;
            }
        }

        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(this);
            if (gameObject.scene.IsValid())
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }
        #endif

        Debug.Log($"Saved {savedCount} placement entries for {phase}.", this);
    }

    private PlacementDictionary GetPlacementsForPhase(PlacementPhase phase)
    {
        switch (phase)
        {
            case PlacementPhase.Phase1:
                return _phase1Placements;
            case PlacementPhase.Phase2:
                return _phase2Placements;
            case PlacementPhase.Phase3:
                return _phase3Placements;
            default:
                return null;
        }
    }

    // public bool TryGetPlacement(GameObject gameObject, out PlacementInfo info)
    // {
    //     if (_placements == null)
    //     {
    //         info = default;
    //         return false;
    //     }

    //     return _placements.TryGetValue(gameObject, out info);
    // }
}
