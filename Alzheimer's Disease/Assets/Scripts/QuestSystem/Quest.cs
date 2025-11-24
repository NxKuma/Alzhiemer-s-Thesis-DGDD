using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO Info;

    public QuestState State;
    private int _currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.Info = questInfo;
        this.State = QuestState.REQUIREMENTS_NOT_MET;
        this._currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrenStepExists()
    {
        return (_currentQuestStepIndex < Info.QuestStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            Object.Instantiate<GameObject>(questStepPrefab, parentTransform);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrenStepExists())
        {
            questStepPrefab = Info.QuestStepPrefabs[_currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Quest Step Prefab Index out of range. No Current step: Quest ID=" + Info.ID + "; Step Index=" + _currentQuestStepIndex);
        }
        return questStepPrefab;
    }
}