using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public string QuestId => questId;
    public int StepIndex => stepIndex;

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
            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            if (DestroyOnFinish)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        GameEventsManager.Instance.questEvents.QuestStepStateChange(
            questId, 
            stepIndex, 
            new QuestStepState(newState, newStatus)
        );
    }

    protected abstract void SetQuestStepState(string state);
}
