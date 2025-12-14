using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField] private NPC _npcData;
    private GameObject _nPCModel;
    private Inventory _npcInventory;
    private GameEventsManager _gameEventsManager;
    private bool _hasInteracted = false;

    private void OnValidate()
    {  
        #if UNITY_EDITOR
        this.name = _npcData != null ? $"{_npcData.GetNPCName()}_NPC" : "NPC_NULL";
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
    
    void Awake()
    {
        _nPCModel = this.transform.GetChild(0).gameObject;
        GameObject npcMesh = Instantiate(_npcData.GetNPCPrefab(), this.transform);
        
        //Make sure the Model is facing forward
        if(_npcData.GetNPCName().Contains("Wife")) npcMesh.transform.rotation = Quaternion.Euler(0,45,0);
        else npcMesh.transform.rotation = Quaternion.Euler(0,90,0);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, -1.2f, this.transform.localPosition.z);
        if(_npcData.GetNPCName().Contains("Daughter")) 
        {
            npcMesh.transform.localScale = Vector3.one * _npcData.GetNPCSize();  
            npcMesh.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = _npcData.GetNPCMaterial();   
        }
        else {
            npcMesh.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = _npcData.GetNPCMaterial();   
        
        }

        while (_nPCModel.transform.childCount > 0)
        {
            Transform child = _nPCModel.transform.GetChild(0);
            child.transform.localPosition = new Vector3(child.transform.localPosition.x, child.transform.localPosition.y + 1f, child.transform.localPosition.z);
            if(child.name.Contains("Face"))
            {
                MeshRenderer rend = child.GetComponent<MeshRenderer>();
                rend.enabled = false;
            }else{  
                child.transform.localScale = Vector3.one * 0.35f;
                child.transform.localPosition = new Vector3(0f, 1.68f, 0f);
            }

            // Preserve the child's world transform (position/rotation/scale) when reparenting
            // so it appears in the same place after being moved under `npcMesh`.
            child.SetParent(npcMesh.transform, worldPositionStays: true);
            
        }
        //Make the capsule invisible
        _nPCModel.SetActive(false);
        //Make sure no camera will be spawned
        foreach(Transform child in npcMesh.transform)
        {
            if (child.GetComponent<Camera>() != null)
                Destroy(child.gameObject);
        }

        // Add a SphereCollider sized to the NPC mesh bounds
        Renderer[] renderers = npcMesh.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;
            foreach (var rend in renderers)
            {
                combinedBounds.Encapsulate(rend.bounds);
            }

            SphereCollider sc = npcMesh.GetComponent<SphereCollider>();
            if (sc == null) sc = npcMesh.AddComponent<SphereCollider>();
            
            // Convert world-space bounds center to local space
            Vector3 localCenter = npcMesh.transform.InverseTransformPoint(combinedBounds.center);
            sc.center = localCenter;
            
            // Radius is half the largest extent of the bounds
            // float maxExtent = Mathf.Max(combinedBounds.size.x, combinedBounds.size.y, combinedBounds.size.z) / 5f;
            sc.radius = 0.3f;
        }
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
        if (_hasInteracted) return;
        // mark as interacted before broadcasting to avoid re-entrant recursion
        _hasInteracted = true;
        Debug.Log("NPC ONINTERACT TRIGGERED (sent from NPC.cs)");
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.NPCInteracted(NPCName);
    } 

    public string GetNPCName() => _npcData.GetNPCName();
    public Sprite GetNPCImage() => _npcData.GetNPCFace();

}
