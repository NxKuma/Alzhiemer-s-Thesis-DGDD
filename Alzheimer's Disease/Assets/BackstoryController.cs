using Unity.VisualScripting;
using UnityEngine;

public class BackstoryController : MonoBehaviour
{
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private CanvasManager _canvasManager;
    [SerializeField] private PuzzleCAnvasScript _puzzleCanvasScript;
    [SerializeField] private TextAsset _backstoryTextJSON;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dialogueManager.EnterDialogueMode(_backstoryTextJSON);
        _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.PuzzleSolving);
        _puzzleCanvasScript.onPuzzleSubmitted += TransitionToDialogue;;
        // GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
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
}
