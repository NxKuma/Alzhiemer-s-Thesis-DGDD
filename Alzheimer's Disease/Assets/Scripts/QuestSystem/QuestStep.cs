using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    private string lastState = "";
    private string lastStatus = "";

    public string QuestId => questId;
    public int StepIndex => stepIndex;

    public bool IsFinishedStep => isFinished;

    protected bool IsFinished => isFinished;
    protected void SetFinishedState(bool finished)
    {
        isFinished = finished;
    }

    protected virtual bool DestroyOnFinish => true;

    public void InitializeQuestStep(string questId, int stepIndex, string questStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            // Persist that this step is finished so UI/history can survive step index rewinds.
            GameEventsManager.Instance.questEvents.QuestStepStateChange(
                questId,
                stepIndex,
                new QuestStepState("FINISHED", lastStatus)
            );

            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            if (DestroyOnFinish)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        lastState = newState;
        lastStatus = newStatus;
        GameEventsManager.Instance.questEvents.QuestStepStateChange(
            questId, 
            stepIndex, 
            new QuestStepState(newState, newStatus)
        );
    }

    protected abstract void SetQuestStepState(string state);
}
