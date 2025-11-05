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
        IdleItem,
        JigsawPuzzle,
        Letter
    }

    public eItemType itemType;

    [Header("Item Mesh Data")]
    [SerializeField] private Mesh _itemMesh;
    [SerializeField] private Material[] _itemMaterial;
    [SerializeField] private Texture2D _itemThumbnail;
    [SerializeField] private float _itemSize;

    //Getters
    public string GetItemName() => _itemName; 
    public string GetItemDesc() => _itemDescription; 
    public eItemType GetItemtype() => itemType; 
    public Mesh GetItemMesh() => _itemMesh; 
    public Material[] GetItemMaterial() => _itemMaterial; 
    public Texture2D GetItemTexture() => _itemThumbnail; 
    public float GetItemSize() => _itemSize;



}
