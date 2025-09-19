using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("Item Data")]
    [SerializeField]
    private string _itemName;
    [SerializeField]
    [TextAreaAttribute(5, 10)]
    private string _itemDescription;

    public enum eItemType
    {
        MainQuestItem,
        PickUp
    }

    public eItemType itemType;

    [Header("Item Mesh Data")]
    [SerializeField] private Mesh _itemMesh;
    [SerializeField] private Material _itemMaterial;
    [SerializeField] private Texture _itemThumbnail;

    public string GetItemName() { return _itemName; }
    public string GetItemDesc() { return _itemDescription; }
    public eItemType GetItemtype() { return itemType; }
    public Mesh GetItemMesh() { return _itemMesh; }
    public Material GetItemMaterial() { return _itemMaterial; }
    public Texture GetItemTexture() { return _itemThumbnail; }





}
