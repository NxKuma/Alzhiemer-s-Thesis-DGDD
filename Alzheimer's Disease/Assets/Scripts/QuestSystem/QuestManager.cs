using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> _questMap;

    private void Awake()
    {
        _questMap = CreateQuestMap();
    }


    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;
    }


    private void Start()
    {
        foreach (Quest q in _questMap.Values)
        {
            GameEventsManager.Instance.questEvents.QuestStateChange(q);
        }
    }


    private void StartQuest(string id)
    {
        // TODO: start quest
        Debug.Log("Start Quest: " + id);
    }

    private void AdvanceQuest(string id)
    {
        // TODO: advance quest
        Debug.Log("Advance Quest: " + id);
    }

    private void FinishQuest(string id)
    {
        // TODO: finish quest
        Debug.Log("Finish Quest: " + id);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // Load all QuestInfoSO from Assets/Resources/Quests
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        // Create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.questID))
            {
                Debug.LogWarning("Duplicate Quest Info ID: " + questInfo.questID);
            }
            idToQuestMap.Add(questInfo.questID, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestByID(string id)
    {
        Quest q = _questMap[id];
        if (q == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return q;
    }
}