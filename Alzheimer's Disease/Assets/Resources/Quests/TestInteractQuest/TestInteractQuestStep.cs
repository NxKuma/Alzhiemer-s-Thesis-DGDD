using UnityEngine;

public class TestInteractQuestStep : QuestStep
{
    private int _npcInteracted = 0;
    private int _npcsToInteract = 2;

    private void OnEnable()
    {
        GameEventsManager.instance.npcEvents.onNPCInteract += NPCInteracted;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.npcEvents.onNPCInteract -= NPCInteracted;
    }

    private void NPCInteracted()
    {
        if (_npcInteracted < _npcsToInteract)
        {
            _npcInteracted++;
        }

        if (_npcInteracted >= _npcsToInteract)
        {
            FinishQuestStep();
        }
    }
}
