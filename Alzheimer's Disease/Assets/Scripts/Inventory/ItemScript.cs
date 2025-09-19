using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private Item _itemResource;

    void Awake()
    {
        if (_itemResource.GetItemMesh() != null) { GetComponent<MeshFilter>().mesh = _itemResource.GetItemMesh(); }
        if (_itemResource.GetItemMaterial() != null) { GetComponent<Renderer>().material = _itemResource.GetItemMaterial(); }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
