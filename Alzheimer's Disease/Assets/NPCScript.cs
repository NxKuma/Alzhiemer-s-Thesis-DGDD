            using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField] private NPC _npcData;
    private GameObject _nPCModel;

    void Awake()
    {
        _nPCModel = this.transform.GetChild(0).gameObject;
        GameObject npcMesh = Instantiate(_npcData.GetNPCPrefab(), this.transform);
        //Make sure the Model is facing forward
        if(_npcData.GetNPCName().Contains("Wife")) npcMesh.transform.rotation = Quaternion.Euler(0,45,0);
        else npcMesh.transform.rotation = Quaternion.Euler(0,90,0);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 1.5f, this.transform.localPosition.z);
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

        


        // MeshFilter mf = _nPCModel.GetComponent<MeshFilter>();
        // mf.mesh = _npcData.GetNPCMesh();
        // Renderer rend = _nPCModel.GetComponent<Renderer>();
        // rend.material = _npcData.GetNPCMaterial();
        // _nPCModel.transform.localScale = Vector3.one * _npcData.GetNPCSize();
    }

    

    void Start()
    {
        
    }


}
