using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
// using UnityEngine.UIElements;

public class QuestLogUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private QuestLogScrollingList scrollingList;
    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questStatusText;
    [SerializeField] private FirstPersonController _player;

    private Button firstSelectedButton;
    private CanvasGroup _canvasGroup;
    private CanvasManager _canvasManager;

    private void Start( )
    {
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        _canvasManager = CanvasManager.Instance;
        _canvasGroup.alpha = 0f; // start hidden
        GameEventsManager.Instance.inputEvents.onQuestLogTogglePressed += QuestLogTogglePressed;
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    // private void OnEnable()
    // {
        
    // }

    // private void OnDisable()
    // {
    //     GameEventsManager.Instance.inputEvents.onQuestLogTogglePressed -= QuestLogTogglePressed;
    //     GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    // }

    private void QuestLogTogglePressed()
    {
        if (_canvasManager.GetPlayerState() != CanvasManager.EPlayerState.Roam || _canvasManager.GetPlayerState() == CanvasManager.EPlayerState.Paused) return; // don't open quest log if game is paused
        if (contentParent.activeInHierarchy)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        _canvasGroup.alpha = 1f;
        _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.QuestAccess);
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        _player.StopStartPlayer(false);
        // note - this needs to happen after the content parent is set active,
        // or else the onSelectAction won't work as expected
        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    private void HideUI()
    {
        _canvasGroup.alpha = 0f;
        _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.Roam);
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        _player.StopStartPlayer(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void QuestStateChange(Quest quest)
    {
        // add the button to the scrolling list if not already added
        QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () => {
            SetQuestLogInfo(quest);
        });

        // initialize the first selected button if not already so that it's
        // always the top button
        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
        }

        // set the button color based on quest state
        questLogButton.SetState(quest.state);
    }

    private void SetQuestLogInfo(Quest quest)
    {
        // quest name
        questDisplayNameText.text = quest.info.displayName;

        // status
        questStatusText.text = quest.GetFullStatusText();
    }
}
