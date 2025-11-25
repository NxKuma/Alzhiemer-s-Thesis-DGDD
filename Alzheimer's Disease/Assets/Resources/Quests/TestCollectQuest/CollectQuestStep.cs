using UnityEngine;

public class CollectQuestStep : QuestStep
{
    
    private GameEventsManager _gameEventsManager;
    private int _itemsCollected = 0;
    private int _itemsToCollect = 3;

    void Start()
    {
        _gameEventsManager = GameEventsManager.Instance;
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.onItemCollected += ItemCollected;
    }

    private void OnDisable()
    {
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.onItemCollected -= ItemCollected;
    }

    private void ItemCollected()
    {
        if (_itemsCollected < _itemsToCollect)
        {
            _itemsCollected++;
        }

        if (_itemsCollected >= _itemsToCollect)
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // No specific states to handle for this collect quest step.
    }
}
