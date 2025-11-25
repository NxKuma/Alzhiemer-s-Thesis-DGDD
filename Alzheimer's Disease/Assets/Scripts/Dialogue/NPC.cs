using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC", order = 0)]
public class NPC : ScriptableObject
{
    [Header("NPC Data")]
    [SerializeField]
    private string _npcName;
    
    // Area for Other NPC details

    [Header("NPC Mesh Data")]
    [SerializeField] private Mesh _npcMesh;
    [SerializeField] private Material _npcMaterial;
    [SerializeField] private Sprite _npcFace;
    [SerializeField] private float _npcSize;
    [SerializeField] private GameObject _npcPrefab;
    //Getters
    public string GetNPCName() => _npcName; 
    public Mesh GetNPCMesh() => _npcMesh; 
    public Material GetNPCMaterial() => _npcMaterial; 
    public Sprite GetNPCFace() => _npcFace; 
    public float GetNPCSize() => _npcSize;
    public GameObject GetNPCPrefab() => _npcPrefab;
}
