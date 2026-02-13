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
        //Get the Model first 
        _nPCModel = this.transform.GetChild(0).gameObject;
        GameObject npcMesh = Instantiate(_npcData.GetNPCPrefab(), this.transform);
        npcMesh.layer = LayerMask.NameToLayer("NPCFace");
        string npcName = _npcData.GetNPCName();

        //Make sure the Model is facing forward
        if(npcName.Contains("Wife")) npcMesh.transform.rotation = Quaternion.Euler(0,45,0);
        else npcMesh.transform.rotation = Quaternion.Euler(0,90,0);

        //Lower the NPC model to align with the ground plane
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, -1.2f, this.transform.localPosition.z);

        if(npcName.Contains("Daughter")) 
        {
            npcMesh.transform.localScale = Vector3.one * _npcData.GetNPCSize();  
            npcMesh.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = _npcData.GetNPCMaterial();   
        }
        else {
            npcMesh.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = _npcData.GetNPCMaterial();   
        
        }

        // Reparent all children of the original NPC model under the new NPC mesh, adjusting their transforms as needed.
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

        // When reparenting, the NPC mesh may lose its collider, so we add a new one here. We use a SphereCollider sized to the combined bounds of the NPC mesh renderers to ensure it encompasses the whole model for interaction purposes.
        // Add a SphereCollider sized to the NPC mesh bounds
        Renderer[] renderers = npcMesh.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length > 0)
        {
            // Bounds combinedBounds = renderers[0].bounds;
            // foreach (var rend in renderers)
            // {
            //     combinedBounds.Encapsulate(rend.bounds);
            // }

            Bounds combinedBounds = npcMesh.GetComponentInChildren<SkinnedMeshRenderer>().bounds;

            BoxCollider sc = npcMesh.GetComponent<BoxCollider>();
            if (sc == null) sc = npcMesh.AddComponent<BoxCollider>();
            
            // Convert world-space bounds center to local space
            Vector3 localCenter = npcMesh.transform.InverseTransformPoint(combinedBounds.center);
            sc.center = localCenter;
            
            // Set the size of the BoxCollider to match the combined bounds
            Vector3 localSize = npcMesh.transform.InverseTransformVector(combinedBounds.size);
            // sc.size = new Vector3(Mathf.Abs(localSize.x), Mathf.Abs(localSize.y), Mathf.Abs(localSize.z)); // Ensure size is positive
            Vector3 sizeToChange;
            if(npcName.Contains("Wife")) sizeToChange = new Vector3(0.38f, 1.8f, 0.35f);
            else if(npcName.Contains("Daughter")) sizeToChange = new Vector3(0.27f, 1.721977f, 0.25f);
            else sizeToChange = new Vector3(0.33f, 1.932664f, 0.21f);
            sc.size = sizeToChange;
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
