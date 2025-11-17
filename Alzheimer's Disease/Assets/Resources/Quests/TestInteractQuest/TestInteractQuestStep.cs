using UnityEngine;

public class TestInteractQuestStep : QuestStep
{
    private int _npcInteracted = 0;

    private void NPCInteracted()
    {
        if (_npcInteracted > 0)
        {
            FinishQuestStep();
        }
    }
}
