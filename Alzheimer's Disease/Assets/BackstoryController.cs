using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class BackstoryController : MonoBehaviour
{
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private CanvasManager _canvasManager;
    [SerializeField] private PuzzleCAnvasScript _puzzleCanvasScript;
    [SerializeField] private TextAsset _backstoryTextJSON;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BeginBackstoryDialogueNextFrame());
        if (_puzzleCanvasScript != null)
        {
            _puzzleCanvasScript.onPuzzleSubmitted += TransitionToDialogue;
        }
        // GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
    }

    private IEnumerator BeginBackstoryDialogueNextFrame()
    {
        // Wait one frame so manager Start methods complete before we enter dialogue.
        yield return null;

        if (_dialogueManager == null)
        {
            _dialogueManager = DialogueManager.GetInstance();
        }

        if (_dialogueManager == null)
        {
            Debug.LogError("BackstoryController could not find DialogueManager instance.");
            yield break;
        }

        if (_backstoryTextJSON == null)
        {
            Debug.LogError("BackstoryController is missing the backstory Ink JSON TextAsset reference.");
            yield break;
        }

        _dialogueManager.EnterDialogueMode(_backstoryTextJSON);

        if (_canvasManager != null)
        {
            _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.PuzzleSolving);
        }
    }

    private void TransitionToDialogue(float puzzleCompletion)
    {
        Debug.Log("Puzzle submitted with completion: " + puzzleCompletion);
        // Arbitrary threshold for "solved"
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (_puzzleCanvasScript != null)
        {
            _puzzleCanvasScript.onPuzzleSubmitted -= TransitionToDialogue;
        }
    }
}
