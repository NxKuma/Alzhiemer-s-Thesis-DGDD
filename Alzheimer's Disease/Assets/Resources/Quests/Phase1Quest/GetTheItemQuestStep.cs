using UnityEngine;

public class GetTheItemQuestStep: QuestStep
{
    [SerializeField] private Item _itemRequired;
    private TriggerHandler _triggerHandler;

    void Start()
    {
        string status = "I need to get the " + _itemRequired.GetItemName() + ".";
        ChangeState("", status);
        _triggerHandler = TriggerHandler.Instance;
        if (_triggerHandler != null) _triggerHandler.PlayerInventory.ItemAdded += ItemCollected;
        if (_triggerHandler.PlayerInventory.GetItemList().Contains(_itemRequired))
        {
            status = "I already have the " + _itemRequired.GetItemName() + ".";
            ChangeState("", status);
            FinishQuestStep();
        }
    }

    private void OnDisable()
    {
        if (_triggerHandler != null) _triggerHandler.PlayerInventory.ItemAdded -= ItemCollected;
    }

    private void ItemCollected(Item item)
    {
        Debug.Log("Item Collected: " + item.GetItemName());
        Debug.Log($"Required Item: {_itemRequired.GetItemName()} isIn {item.GetItemName()} : {item.GetItemName().Contains(_itemRequired.GetItemName())}"); 
        if (!item.GetItemName().Contains(_itemRequired.GetItemName())) return;
        else
        {
            string status = "I have collected the " + _itemRequired.GetItemName() + ".";
            ChangeState("", status);
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // No specific states to handle for this brush collection quest step.
    }
    
}
