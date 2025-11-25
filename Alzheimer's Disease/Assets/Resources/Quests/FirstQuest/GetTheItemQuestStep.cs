using UnityEngine;

public class GetTheItemQuestStep: QuestStep
{
    [SerializeField] private string _itemRequired;
    private TriggerHandler _triggerHandler;

    void Start()
    {
        _triggerHandler = TriggerHandler.Instance;
        if (_triggerHandler != null) _triggerHandler.PlayerInventory.ItemAdded += ItemCollected;
    }

    private void OnDisable()
    {
        if (_triggerHandler != null) _triggerHandler.PlayerInventory.ItemAdded -= ItemCollected;
    }

    private void ItemCollected(Item item)
    {
        Debug.Log("Item Collected: " + item.GetItemName());
        Debug.Log($"Required Item: {_itemRequired} isIn {item.GetItemName()} : {item.GetItemName().Contains(_itemRequired)}"); 
        if (!item.GetItemName().Contains(_itemRequired)) return;
        else FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        // No specific states to handle for this brush collection quest step.
    }
    
}
