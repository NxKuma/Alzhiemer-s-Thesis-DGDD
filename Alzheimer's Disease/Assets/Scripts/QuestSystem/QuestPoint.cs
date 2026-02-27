using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake()
    {
        if (questInfoForPoint != null)
        {
            questId = questInfoForPoint.id;
        }
    }

    private void Start() 
    {
        if (string.IsNullOrEmpty(questId) && questInfoForPoint != null)
        {
            questId = questInfoForPoint.id;
        }
        questIcon = GetComponentInChildren<QuestIcon>();
        if (questIcon == null)
        {
            Debug.LogWarning($"QuestPoint '{name}': no QuestIcon found in children. Quest icon state updates will be skipped.");
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.Instance.npcEvents.onNPCInteract += NPCInteract;
        GameEventsManager.Instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.Instance.npcEvents.onNPCInteract -= NPCInteract;
        GameEventsManager.Instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void NPCInteract(string npcName)
    {
        // if we have a knot name defined, try to start dialogue with it
        if (!dialogueKnotName.Equals("")) 
        {
            GameEventsManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }// otherwise, start or finish the quest immediately without dialogue
        else 
        {
            // Debug.Log("Player near quest point, processing NPC interact.");
            // start or finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.Instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventsManager.Instance.questEvents.FinishQuest(questId);
            }
        }
    }

    private void SubmitPressed(InputEventContext inputEventContext)
    {
        if (!playerIsNear || !inputEventContext.Equals(InputEventContext.DEFAULT))
        {
            return;
        }

        // if we have a knot name defined, try to start dialogue with it
        if (!dialogueKnotName.Equals("")) 
        {
            GameEventsManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        // otherwise, start or finish the quest immediately without dialogue
        else 
        {
            // start or finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.Instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventsManager.Instance.questEvents.FinishQuest(questId);
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            // only update icon if it exists (null guard)
            if (questIcon != null)
            {
                questIcon.SetState(currentQuestState, startPoint, finishPoint);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
