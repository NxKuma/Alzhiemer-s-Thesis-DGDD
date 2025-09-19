using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
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




}
