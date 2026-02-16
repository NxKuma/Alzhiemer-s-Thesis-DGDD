using UnityEngine;

public class GetTheItemQuestStep: QuestStep
{
    [SerializeField] private Item _itemRequired;
    private TriggerHandler _triggerHandler;

    protected override bool DestroyOnFinish => false;

    void Start()
    {
        string status = "I need to get the " + _itemRequired.GetItemName() + ".";
        ChangeState("", status);
        _triggerHandler = TriggerHandler.Instance;
        if (_triggerHandler != null)
        {
            _triggerHandler.PlayerInventory.ItemAdded += ItemCollected;
            _triggerHandler.PlayerInventory.ItemDropped += ItemDropped;
        }
        if (_triggerHandler.PlayerInventory.GetItemList().Contains(_itemRequired))
        {
            if(TriggerAreaScript.GetItemStatus(_itemRequired) == ItemStatus.Hidden)
            {  
                status = "I already have the " + _itemRequired.GetItemName() + ".";
                ChangeState("", status);
                FinishQuestStep();
            }else if(TriggerAreaScript.GetItemStatus(_itemRequired) == ItemStatus.Dropped)
            {
                status = "I think I misplaced the " + _itemRequired.GetItemName() + "...";
                ChangeState("", status);
            }
        }
    }

    private void OnDisable()
    {
        if (_triggerHandler != null)
        {
            _triggerHandler.PlayerInventory.ItemAdded -= ItemCollected;
            _triggerHandler.PlayerInventory.ItemDropped -= ItemDropped;
        }
    }

    private void ItemCollected(Item item)
    {
        Debug.Log("Item Collected: " + item.GetItemName());
        if (item != _itemRequired) return;

        string status = "I have collected the " + _itemRequired.GetItemName() + ".";
        ChangeState("", status);

        // Once completed, we no longer need to listen for adds.
        if (_triggerHandler != null) _triggerHandler.PlayerInventory.ItemAdded -= ItemCollected;
        FinishQuestStep();
    }

    private void ItemDropped(Item item)
    {
        if (item != _itemRequired) return;

        // Mark this step as unfinished again.
        SetFinishedState(false);
        string status = "I think I misplaced the " + _itemRequired.GetItemName() + "...";
        ChangeState("", status);
        if (_triggerHandler != null) _triggerHandler.PlayerInventory.ItemAdded += ItemCollected;

        // Force the quest back onto this step (this also destroys the current active step object for the quest).
        GameEventsManager.Instance.questEvents.SetQuestStepIndex(QuestId, StepIndex);
    }

    public Item GetItemRequired() => _itemRequired;

    protected override void SetQuestStepState(string state)
    {
        // No specific states to handle for this brush collection quest step.
    }
    
}
