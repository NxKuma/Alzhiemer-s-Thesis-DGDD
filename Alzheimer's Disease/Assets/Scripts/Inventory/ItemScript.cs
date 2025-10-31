using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private Item _itemResource;

    void Awake()
    {
        // this.gameObject.SetActive(false); //This is for future gameplay things
        float size = _itemResource.GetItemSize();
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
        transform.localScale *= size;

    }

    public Item GetItemResource() => _itemResource;

   public void Initialize(Item item)
    {
        _itemResource = item;

        if (_itemResource != null)
        {
            if (_itemResource.GetItemMesh() != null)
            {
                MeshFilter mf = GetComponent<MeshFilter>();
                if (mf != null) mf.mesh = _itemResource.GetItemMesh();
            }

            Renderer rend = GetComponent<Renderer>();
            if (rend != null && _itemResource.GetItemMaterial() != null)
                rend.material = _itemResource.GetItemMaterial();
        }
    }
}
