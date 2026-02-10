using UnityEngine;

public class TalkToNPC : QuestStep
{
    [SerializeField] private string _npcName;
    [Header("Ink (optional)")]
    [Tooltip("Name of a bool in globals.ink (ex: p1q1s1done). If true, this quest step auto-completes.")]
    [SerializeField] private string _inkBoolVariableName;

    [Tooltip("If set to >= 0, this step auto-completes when the given Ink int variable reaches this value (ex: gamePhase >= 2).")]
    [SerializeField] private int _autoCompleteWhenInkIntAtLeast = -1;

    [Tooltip("If set to >= 0, this step auto-completes when the given Ink float variable reaches this value (ex: gamePhase >= 2.5).")]
    [SerializeField] private float _autoCompleteWhenInkFloatAtLeast = -1f;

    [Tooltip("Ink int variable name to compare against _autoCompleteWhenInkIntAtLeast.")]
    [SerializeField] private string _inkIntVariableName = "gamePhase";

    [Tooltip("If true, when auto-completed via phase/int threshold, also sets _inkBoolVariableName=true (if provided).")]
    [SerializeField] private bool _setBoolWhenAutoCompletedByPhase = true;

    private GameEventsManager _gameEventsManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string status = "I need to talk to " + _npcName;
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
        
        if (!npcName.Contains(_npcName)) return;
        else{
            MarkTalkedToNpcInInk();
            string status = _npcName + ". hmmm I remember talking to them.";
            ChangeState("", status);
            FinishQuestStep();
        }
    }

    private bool HasAlreadyTalkedToNpc()
    {
        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager == null)
        {
            return false;
        }

        // 1) If a specific "talked" bool is configured, it wins.
        if (!string.IsNullOrWhiteSpace(_inkBoolVariableName) && dialogueManager.GetGlobalInkBool(_inkBoolVariableName, false))
        {
            return true;
        }

        // 2) Optionally treat it as already done when game phase (or any int var) has advanced.
        if (_autoCompleteWhenInkFloatAtLeast >= 0f && !string.IsNullOrWhiteSpace(_inkIntVariableName))
        {
            float currentValue = dialogueManager.GetGlobalInkFloat(_inkIntVariableName, float.NaN);
            if (!float.IsNaN(currentValue) && currentValue >= _autoCompleteWhenInkFloatAtLeast)
            {
                if (_setBoolWhenAutoCompletedByPhase)
                {
                    MarkTalkedToNpcInInk();
                }
                return true;
            }
        }

        // Back-compat: if you already configured the int threshold, keep supporting it.
        if (_autoCompleteWhenInkIntAtLeast >= 0 && !string.IsNullOrWhiteSpace(_inkIntVariableName))
        {
            int currentValue = dialogueManager.GetGlobalInkInt(_inkIntVariableName, int.MinValue);
            if (currentValue != int.MinValue && currentValue >= _autoCompleteWhenInkIntAtLeast)
            {
                if (_setBoolWhenAutoCompletedByPhase)
                {
                    MarkTalkedToNpcInInk();
                }
                return true;
            }
        }

        return false;
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