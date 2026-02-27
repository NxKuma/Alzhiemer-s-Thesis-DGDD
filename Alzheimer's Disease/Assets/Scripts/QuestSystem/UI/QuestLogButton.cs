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
    private SFXManager sFXManager;
    private Navigation _defaultNavigation;
    // because we're instantiating the button and it may be disabled when we
    // instantiate it, we need to manually initialize anything here.
    public void Initialize(string displayName, UnityAction selectAction) 
    {
        this.button = this.GetComponent<Button>();
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();

        this.buttonText.text = displayName;
        this.onSelectAction = selectAction;
        sFXManager = SFXManager.Instance;
        _defaultNavigation = button.navigation; // cache the default navigation settings for when we want to re-enable selection
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
        _ = sFXManager.PlaySFX("TurnPage");
    }

    // NEW: centralize "can this entry be navigated/selected?"
    private void SetSelectable(bool selectable)
    {
        // Visuals are handled elsewhere; this is about selection/navigation.
        canvasGroup.interactable = selectable;
        canvasGroup.blocksRaycasts = selectable;

        // Important for keyboard navigation:
        // Selectable navigation skips non-interactable items.
        button.interactable = selectable;

        // Extra safety: prevent it from being part of explicit navigation graphs
        if (selectable)
        {

            button.navigation = _defaultNavigation;

        }
        else
        {
            var nav = button.navigation;
            nav.mode = Navigation.Mode.None;
            button.navigation = nav;
        }
    }

    public void SetState(QuestState state)
    {
        switch (state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                canvasGroup.alpha = 0f;

                // NEW: make truly non-scrollable/non-selectable when invisible
                SetSelectable(false);
                break;

            case QuestState.CAN_START:
                buttonText.color = Color.red * Color.yellow;
                canvasGroup.alpha = 0.9f;

                // NEW
                SetSelectable(true);
                break;

            case QuestState.IN_PROGRESS:
            case QuestState.CAN_FINISH:
                buttonText.color = Color.yellow;
                canvasGroup.alpha = 1f;

                // NEW
                SetSelectable(true);
                break;

            case QuestState.FINISHED:
                buttonText.color = Color.green;
                canvasGroup.alpha = 1f;

                // NEW
                SetSelectable(true);
                break;

            default:
                Debug.LogWarning("Quest State not recognized by switch statement: " + state);
                break;
        }
    }
}