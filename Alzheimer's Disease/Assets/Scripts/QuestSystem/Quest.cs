using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    // static info
    public QuestInfoSO info;

    // state info
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
        this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        this.info = questInfo;
        this.state = questState;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStates = questStepStates;

        // if the quest step states and prefabs are different lengths,
        // something has changed during development and the saved data is out of sync.
        if (this.questStepStates.Length != this.info.questStepPrefabs.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                + "of different lengths. This indicates something changed "
                + "with the QuestInfo and the saved data is now out of sync. "
                + "Reset your data - as this might cause issues. QuestId: " + this.info.id);
        }
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool AdvanceToNextUnfinishedStep()
    {
        currentQuestStepIndex++;
        while (CurrentStepExists()
            && questStepStates[currentQuestStepIndex] != null
            && questStepStates[currentQuestStepIndex].state == "FINISHED")
        {
            currentQuestStepIndex++;
        }
        return CurrentStepExists();
    }

    public void SetCurrentStepIndex(int stepIndex)
    {
        currentQuestStepIndex = Mathf.Clamp(stepIndex, 0, info.questStepPrefabs.Length);
    }

    public int GetCurrentStepIndex()
    {
        return currentQuestStepIndex;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else 
        {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + info.id + ", stepIndex=" + currentQuestStepIndex);
        }
        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
            questStepStates[stepIndex].status = questStepState.status;
        }
        else 
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                + "Quest Id = " + info.id + ", Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }

    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (state == QuestState.REQUIREMENTS_NOT_MET)
        {
            fullStatus = "I don't know what to do with this yet.";
        }
        else if (state == QuestState.CAN_START)
        {
            fullStatus = "I have to find someone!";
        }
        else 
        {
            // Display steps using explicit FINISHED state so rewinds don't "unfinish" history.
            for (int i = 0; i < questStepStates.Length; i++)
            {
                if (questStepStates[i] == null) continue;
                if (string.IsNullOrWhiteSpace(questStepStates[i].status)) continue;

                bool isCurrent = (i == currentQuestStepIndex) && CurrentStepExists();
                bool isFinishedStep = questStepStates[i].state == "FINISHED";

                if (isCurrent)
                {
                    fullStatus += questStepStates[i].status + "\n";
                }
                else if (isFinishedStep)
                {
                    fullStatus += "<s>" + questStepStates[i].status + "</s>\n";
                }
            }

            // If we have a current step but no status yet, fall back to the old behavior.
            if (CurrentStepExists() && string.IsNullOrWhiteSpace(questStepStates[currentQuestStepIndex].status))
            {
                fullStatus += questStepStates[currentQuestStepIndex].status;
            }
            // when the quest is completed or turned in
            if (state == QuestState.CAN_FINISH)
            {
                fullStatus += "I think I have all that I need.";
            }
            else if (state == QuestState.FINISHED)
            {
                fullStatus += "I did this already...";
            }
        }

        return fullStatus;
    }
}
