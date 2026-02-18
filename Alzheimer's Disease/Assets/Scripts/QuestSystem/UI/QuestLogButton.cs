using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button button { get; private set; }
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI buttonText;
    private UnityAction onSelectAction;

    // because we're instantiating the button and it may be disabled when we
    // instantiate it, we need to manually initialize anything here.
    public void Initialize(string displayName, UnityAction selectAction) 
    {
        this.button = this.GetComponent<Button>();
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();

        this.buttonText.text = displayName;
        this.onSelectAction = selectAction;
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }

    public void SetState(QuestState state)
    {
        switch (state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                canvasGroup.alpha = 0f;
                break;
            case QuestState.CAN_START:
                buttonText.color = Color.red * Color.yellow;
                canvasGroup.alpha = 0.9f;
                break;
            case QuestState.IN_PROGRESS:
            case QuestState.CAN_FINISH:
                buttonText.color = Color.yellow;
                canvasGroup.alpha = 1f;
                break;
            case QuestState.FINISHED:
                buttonText.color = Color.green;
                canvasGroup.alpha = 1f;
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement: " + state);
                break;
        }
    }
}
