using UnityEngine;

public class TestInteractQuestStep : QuestStep
{
    private GameEventsManager _gameEventsManager;
    private int _npcInteracted = 0;
    private int _npcsToInteract = 2;

    void Start()
    {
        _gameEventsManager = GameEventsManager.Instance;
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.onNPCInteract += NPCInteracted;
    }

    private void OnDisable()
    {
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.onNPCInteract -= NPCInteracted;
    }

    private void NPCInteracted()
    {
        if (_npcInteracted < _npcsToInteract)
        {
            _npcInteracted++;
        }

        if (_npcInteracted > _npcsToInteract)
        {
            FinishQuestStep();
        }
    }
}
