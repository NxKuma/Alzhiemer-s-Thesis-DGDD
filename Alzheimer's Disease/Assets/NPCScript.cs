using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField] private NPC _npcData;
    [SerializeField] private float _meshYOffset = -1.2f;
    private GameObject _nPCModel;
    private Inventory _npcInventory;
    private GameEventsManager _gameEventsManager;
    private bool _hasInteracted = false;
    private const string PreviewMeshName = "__NPC_PREVIEW_MESH";

    private void OnValidate()
    {  
        #if UNITY_EDITOR
        this.name = _npcData != null ? $"{_npcData.GetNPCName()}_NPC" : "NPC_NULL";

        if (!Application.isPlaying)
        {
            BuildEditorPreviewMesh();
        }

        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
    
    void Awake()
    {
        RemovePreviewMesh();

        if (_npcData == null || _npcData.GetNPCPrefab() == null)
        {
            Debug.LogWarning($"NPC data or prefab missing on {name}");
            return;
        }

        if (!TryGetBaseModel(out _nPCModel))
        {
            Debug.LogWarning($"No base NPC model found on {name}");
            return;
        }
        string npcName = _npcData.GetNPCName();

        GameObject npcMesh = Instantiate(_npcData.GetNPCPrefab(), transform, false);
        npcMesh.layer = LayerMask.NameToLayer("NPCFace");
        npcMesh.transform.localPosition = new Vector3(0f, _meshYOffset, 0f);
        npcMesh.transform.localRotation = IsWife(npcName)
            ? Quaternion.Euler(0f, 45f, 0f)
            : Quaternion.Euler(0f, 90f, 0f);

        if (IsDaughter(npcName))
        {
            npcMesh.transform.localScale = Vector3.one * _npcData.GetNPCSize();
        }

        ApplyNpcMaterial(npcMesh, npcName);
        ReparentLegacyChildren(npcMesh.transform);

        _nPCModel.SetActive(false);

        foreach (Transform child in npcMesh.transform)
        {
            if (child.GetComponent<Camera>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        SetupCollider(npcMesh, npcName);
    }

    private bool TryGetBaseModel(out GameObject baseModel)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name == PreviewMeshName) continue;

            baseModel = child.gameObject;
            return true;
        }

        baseModel = null;
        return false;
    }

    private void RemovePreviewMesh()
    {
        Transform preview = transform.Find(PreviewMeshName);
        if (preview == null) return;

        if (Application.isPlaying)
        {
            Destroy(preview.gameObject);
            return;
        }

#if UNITY_EDITOR
        GameObject previewObject = preview.gameObject;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (previewObject != null)
            {
                DestroyImmediate(previewObject);
            }
        };
#endif
    }

#if UNITY_EDITOR
    private void BuildEditorPreviewMesh()
    {
        if (_npcData == null || _npcData.GetNPCPrefab() == null)
        {
            RemovePreviewMesh();
            return;
        }

        Transform existingPreview = transform.Find(PreviewMeshName);
        GameObject previewMesh = existingPreview != null
            ? existingPreview.gameObject
            : Instantiate(_npcData.GetNPCPrefab(), transform, false);

        previewMesh.name = PreviewMeshName;
        previewMesh.layer = LayerMask.NameToLayer("NPCFace");
        previewMesh.transform.localPosition = new Vector3(0f, _meshYOffset, 0f);

        string npcName = _npcData.GetNPCName();
        previewMesh.transform.localRotation = IsWife(npcName)
            ? Quaternion.Euler(0f, 45f, 0f)
            : Quaternion.Euler(0f, 90f, 0f);

        if (IsDaughter(npcName))
        {
            previewMesh.transform.localScale = Vector3.one * _npcData.GetNPCSize();
        }
        else
        {
            previewMesh.transform.localScale = Vector3.one;
        }

        ApplyNpcMaterial(previewMesh, npcName);

        foreach (Transform child in previewMesh.transform)
        {
            Camera childCamera = child.GetComponent<Camera>();
            if (childCamera != null)
            {
                childCamera.enabled = false;
            }
        }
    }
#endif

    private void ApplyNpcMaterial(GameObject npcMesh, string npcName)
    {
        int materialChildIndex = IsDaughter(npcName) ? 0 : 1;
        if (npcMesh.transform.childCount <= materialChildIndex) return;

        SkinnedMeshRenderer renderer = npcMesh.transform.GetChild(materialChildIndex).GetComponent<SkinnedMeshRenderer>();
        if (renderer == null) return;

        renderer.material = _npcData.GetNPCMaterial();
    }

    private void ReparentLegacyChildren(Transform npcMeshTransform)
    {
        while (_nPCModel.transform.childCount > 0)
        {
            Transform child = _nPCModel.transform.GetChild(0);
            child.SetParent(npcMeshTransform, false);

            if (child.name.Contains("Face"))
            {
                child.localPosition += Vector3.up;
                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                if (renderer != null) renderer.enabled = false;
                continue;
            }

            child.localScale = Vector3.one * 0.35f;
            child.localPosition = new Vector3(0f, 1.68f, 0f);
        }
    }

    private void SetupCollider(GameObject npcMesh, string npcName)
    {
        Renderer[] renderers = npcMesh.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        BoxCollider collider = npcMesh.GetComponent<BoxCollider>();
        if (collider == null) collider = npcMesh.AddComponent<BoxCollider>();

        collider.center = npcMesh.transform.InverseTransformPoint(combinedBounds.center);

        Vector3 fallbackSize = npcMesh.transform.InverseTransformVector(combinedBounds.size);
        fallbackSize = new Vector3(Mathf.Abs(fallbackSize.x), Mathf.Abs(fallbackSize.y), Mathf.Abs(fallbackSize.z));
        collider.size = GetColliderSizeOverride(npcName, fallbackSize);
    }

    private static Vector3 GetColliderSizeOverride(string npcName, Vector3 fallbackSize)
    {
        if (IsWife(npcName)) return new Vector3(0.38f, 1.8f, 0.35f);
        if (IsDaughter(npcName)) return new Vector3(0.27f, 1.721977f, 0.25f);
        if (!string.IsNullOrWhiteSpace(npcName)) return new Vector3(0.33f, 1.932664f, 0.21f);
        return fallbackSize;
    }

    private static bool IsWife(string npcName)
    {
        return !string.IsNullOrWhiteSpace(npcName) && npcName.IndexOf("Wife", System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static bool IsDaughter(string npcName)
    {
        return !string.IsNullOrWhiteSpace(npcName) && npcName.IndexOf("Daughter", System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void Start()
    {
        _gameEventsManager = GameEventsManager.Instance;
        if (_gameEventsManager != null) _gameEventsManager.npcEvents.onNPCInteract += OnInteract;
    }

    private void OnDestroy()
    {
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.onNPCInteract -= OnInteract;
    }

    public void Interact()
    {
        // Debug.Log("NPC INTERACTED (sent from NPC.cs)"); 
        // GameEventsManager.Instance.npcEvents.NPCInteracted();
        // do something
    }

    public void OnInteract(string NPCName)
    {   
        // This listener should not re-broadcast the event back onto the bus.
        // Re-emitting here causes recursive / duplicated NPC interaction events.
        if (string.IsNullOrWhiteSpace(NPCName)) return;
        if (!_npcData.GetNPCName().Equals(NPCName)) return;
        if (_hasInteracted) return;

        _hasInteracted = true;
        Debug.Log("NPC ONINTERACT TRIGGERED");
    } 

    public string GetNPCName() => _npcData.GetNPCName();
    // public Sprite GetNPCImage() => _npcData.GetNPCFace();
    public Sprite GetNPCImage(string emotion)
    {
        return _npcData.GetNPCEmotions(emotion);
    }

    public Sprite[] GetNPCImages()
    {
        return _npcData.GetNPCEmotions();
    }

}
