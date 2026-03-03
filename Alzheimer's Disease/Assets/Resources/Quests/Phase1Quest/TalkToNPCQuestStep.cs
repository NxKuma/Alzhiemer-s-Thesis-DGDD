using UnityEngine;

public class TalkToNPC : QuestStep
{
    [SerializeField] private string _npcName;
    [Header("Ink (optional)")]
    [Tooltip("Name of a bool in globals.ink (ex: p1q1s1done). If true, this quest step auto-completes.")]
    [SerializeField] private string _inkBoolVariableName;

    private GameEventsManager _gameEventsManager;

    private static string NormalizeNpcName(string npcName)
    {
        if (string.IsNullOrWhiteSpace(npcName))
        {
            return string.Empty;
        }

        return npcName
            .Replace("_NPC", "", System.StringComparison.OrdinalIgnoreCase)
            .Replace("_", " ")
            .Replace("-", " ")
            .Trim()
            .ToLowerInvariant();
    }

    private bool IsTargetNpc(string interactedNpcName)
    {
        string targetName = NormalizeNpcName(_npcName);
        string currentName = NormalizeNpcName(interactedNpcName);

        if (string.IsNullOrEmpty(targetName) || string.IsNullOrEmpty(currentName))
        {
            return false;
        }

        return string.Equals(currentName, targetName, System.StringComparison.Ordinal);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (!string.IsNullOrWhiteSpace(_npcName) && _npcName.Contains("Daughter")) _npcName = "Daughter-in-Law";

        string status = "I need to talk to my " + _npcName;
        ChangeState("", status);

        if (HasAlreadyTalkedToNpc())
        {
            status = _npcName + ". hmmm I remember talking to them.";
            ChangeState("", status);
            FinishQuestStep();
            return;
        }

        _gameEventsManager = GameEventsManager.Instance;
        if (_gameEventsManager != null) _gameEventsManager.npcEvents.onNPCInteract += NPCInteracted;
    }

    private void OnDisable()
    {
        if (_gameEventsManager != null) _gameEventsManager.npcEvents.onNPCInteract -= NPCInteracted;
    }

    private void NPCInteracted(string npcName)
    {
        if (!IsTargetNpc(npcName)) return;

        MarkTalkedToNpcInInk();
        string status = _npcName + ". hmmm I remember talking to them.";
        ChangeState("", status);
        FinishQuestStep();
    }

    private bool HasAlreadyTalkedToNpc()
    {
        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(_inkBoolVariableName))
        {
            return false;
        }

        return dialogueManager.GetGlobalInkBool(_inkBoolVariableName, false);
    }

    private void MarkTalkedToNpcInInk()
    {
        if (string.IsNullOrWhiteSpace(_inkBoolVariableName))
        {
            return;
        }

        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager == null)
        {
            return;
        }

        dialogueManager.SetGlobalInkBool(_inkBoolVariableName, true);
    }

    protected override void SetQuestStepState(string state)
    {
        // No specific states to handle for this brush collection quest step.
    }
    
}