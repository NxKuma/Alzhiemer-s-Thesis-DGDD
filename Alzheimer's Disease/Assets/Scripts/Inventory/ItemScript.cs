using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private Item _itemResource;

    void Awake()
    {
        // this.gameObject.SetActive(false);

        Mesh mesh = _itemResource.GetItemMesh();
        if (_itemResource.GetItemMesh() != null)
        {
            GetComponent<MeshFilter>().mesh = mesh;

            BoxCollider bc = GetComponent<BoxCollider>();
            if (bc == null) bc = gameObject.AddComponent<BoxCollider>();
            bc.center = mesh.bounds.center;
            bc.size = mesh.bounds.size;
        }

        if (_itemResource.GetItemMaterial() != null)
        {
            GetComponent<Renderer>().material = _itemResource.GetItemMaterial();
        }

    }

    public Item GetItemResource() => _itemResource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
