using UnityEngine;

public class TalkToNPC : QuestStep
{
    [SerializeField] private string _npcName;
    private GameEventsManager _gameEventsManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string status = "I need to talk to " + _npcName;
        ChangeState("", status);
        _gameEventsManager = GameEventsManager.Instance;
        if (_gameEventsManager != null) _gameEventsManager.npcEvents.onNPCInteract += NPCInteracted;
    }

    private void OnDisable()
    {
        if (_gameEventsManager != null) _gameEventsManager.npcEvents.onNPCInteract -= NPCInteracted;
    }

    private void NPCInteracted(string npcName)
    {
        
        if (!npcName.Contains(_npcName)) return;
        else{
            string status = _npcName + ". hmmm I remember talking to them.";
            ChangeState("", status);
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // No specific states to handle for this brush collection quest step.
    }
    
}