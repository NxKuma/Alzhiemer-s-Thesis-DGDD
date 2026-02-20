using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;
    [SerializeField] private TriggerHandler _triggerHandlerPrefab;

    private Dictionary<string, Quest> questMap;

    // quest start requirements
    private int currentPlayerLevel;

    // private void Start()
    // {
        
    // }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onSetQuestStepIndex += SetQuestStepIndex;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onSetQuestStepIndex -= SetQuestStepIndex;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

    }

    private void Start()
    {
        questMap = CreateQuestMap();
        foreach (Quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.Instance.questEvents.QuestStateChange(quest);
    }

    private void PlayerLevelChange(int level)
    {
        currentPlayerLevel = level;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        // start true and prove to be false
        bool meetsRequirements = true;
        // return true;
        
        // Treat null/empty prerequisites as "no requirements"
        if (quest.info.questPrerequisites == null || quest.info.questPrerequisites.Length <= 0)
        {
            return meetsRequirements;
        }

        // check quest prerequisites for completion
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (prerequisiteQuestInfo == null) continue;
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        // loop through ALL quests
        foreach (Quest quest in questMap.Values)
        {
            // if we're now meeting the requirements, switch over to the CAN_START state
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id) 
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        // Move on to the next unfinished step (skipping any steps already completed).
        if (quest.AdvanceToNextUnfinishedStep())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
            return;
        }

        // No unfinished steps remain.
        ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
    }

    private void SetQuestStepIndex(string id, int stepIndex)
    {
        Quest quest = GetQuestById(id);

        // if the quest is already finished, don't allow regressions
        if (quest.state == QuestState.FINISHED)
        {
            return;
        }

        quest.SetCurrentStepIndex(stepIndex);

        // Destroy any active quest step objects for this quest that are not the target step.
        QuestStep[] activeSteps = this.transform.GetComponentsInChildren<QuestStep>(true);
        bool hasTargetStepObject = false;
        foreach (QuestStep activeStep in activeSteps)
        {
            if (activeStep == null || activeStep.QuestId != id)
            {
                continue;
            }

            if (activeStep.StepIndex == stepIndex)
            {
                hasTargetStepObject = true;
                continue;
            }

            // Keep completed steps in the scene (some steps use DestroyOnFinish=false).
            // This prevents rewinds from wiping already-finished step objects.
            if (!activeStep.IsFinishedStep)
            {
                Destroy(activeStep.gameObject);
            }
        }

        // Ensure the target step exists in the scene.
        if (quest.CurrentStepExists() && !hasTargetStepObject)
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }

        ChangeQuestState(id, quest.state);
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

        // Get the QuestSO for this quest and check the GetItemQuestSteps to mark them complete so that it can be removed in the inventory.
        QuestInfoSO questInfo = quest.info;
        foreach (GameObject itemQuest in questInfo.questStepPrefabs)
        {
            if (itemQuest.GetComponent<GetTheItemQuestStep>() != null)
            {
                GetTheItemQuestStep getTheItemQuestStep = itemQuest.GetComponent<GetTheItemQuestStep>();
                _triggerHandlerPrefab.PlayerInventory.Inventory_CompleteItem(getTheItemQuestStep.GetItemRequired());
            }
        }

    }


    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);

        // If a quest was marked FINISHED, but one of its steps becomes unfinished again
        // (e.g., the player drops a required item), reopen the quest.
        if (quest.state == QuestState.CAN_FINISH
            && questStepState != null)
        {
            quest.SetCurrentStepIndex(stepIndex);
            quest.state = QuestState.IN_PROGRESS;
        }

        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // loads all QuestInfoSO Scriptable Objects under the Assets/Resources/Quests folder
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        // Create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest)
    {
        try 
        {
            QuestData questData = quest.GetQuestData();
            // serialize using JsonUtility, but use whatever you want here (like JSON.NET)
            string serializedData = JsonUtility.ToJson(questData);
            // saving to PlayerPrefs is just a quick example for this tutorial video,
            // you probably don't want to save this info there long-term.
            // instead, use an actual Save & Load system and write to a file, the cloud, etc..
            PlayerPrefs.SetString(quest.info.id, serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
        }
    }

    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try 
        {
            // load quest from saved data
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            // otherwise, initialize a new quest
            else 
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }
}
