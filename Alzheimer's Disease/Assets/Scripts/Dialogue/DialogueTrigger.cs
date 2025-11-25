using UnityEngine;

/* 
* This script handles the interaction between
* the player character and the NPC's dialogue
*/
public class DialogueTrigger : MonoBehaviour
{
    [Header("InteractUI")]
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private Transform _interactorSource;
    [SerializeField] private float _interactRange = 2.0f;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset _inkJSON;

    private bool _playerInRange;
    private CanvasGroup _uiCanvasGroup;
    [HideInInspector] public string npcName;
    private GameEventsManager _gameEventsManager;
    private PuzzleCAnvasScript _puzzleCanvasScript;
    private DialogueManager _dialogueManager;

    // [Header("Debug - isolate leak")]
    // [Tooltip("If false, the Physics.Raycast call will be skipped (no hit).")]
    // [SerializeField] private bool _dbgAllowRaycast = true;
    // [Tooltip("If false, GameEventsManager.npcEvents.NPCInteracted() won't be called.")]
    // [SerializeField] private bool _dbgCallEvent = true;
    // [Tooltip("If false, npc.Interact() won't be called.")]
    // [SerializeField] private bool _dbgCallNpcInteract = true;
    // [Tooltip("If false, DialogueManager.EnterDialogueMode won't be called.")]
    // [SerializeField] private bool _dbgCallEnterDialogue = true;

    private void Awake()
    {
        _playerInRange = false;
    }

    private void Start()
    {
        _gameEventsManager = GameEventsManager.Instance;
        _puzzleCanvasScript = PuzzleCAnvasScript.Instance;
        _dialogueManager = DialogueManager.GetInstance();
        // _interactorSource = GameObject.FindWithTag("Player").transform;
        // _interactUI = GameObject.FindWithTag("InteractUI");
    }

    private void Update()
    {
        if (_playerInRange && !_dialogueManager.DialogueIsPlaying)
        {
            _interactUI.GetComponent<CanvasGroup>().alpha = 1f;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray r = new Ray(_interactorSource.position, _interactorSource.forward);
                Debug.DrawRay(r.origin, r.direction * _interactRange, Color.red, 2.0f);
                int mask = LayerMask.GetMask("NPC");

                if (Physics.Raycast(r, out RaycastHit hitInfo, _interactRange))
                {
                    Debug.Log("RAYCAST HIT: " + hitInfo.collider.name);
                    NPCScript npc = hitInfo.collider.GetComponentInParent<NPCScript>();
                    if (npc != null)
                    {
                            npcName = npc.GetNPCName();
                            _puzzleCanvasScript.SetNPCName(npcName);
                            _gameEventsManager.npcEvents.NPCInteracted();
                            npc.Interact();
                            _dialogueManager.EnterDialogueMode(_inkJSON);
                        // if (_dbgCallEvent)
                        // {
                        // }
                        // else Debug.Log("[DialogueTrigger] NPCInteracted skipped (debug)");

                        // if (_dbgCallNpcInteract)
                        // {   
                        // }
                        // else Debug.Log("[DialogueTrigger] npc.Interact skipped (debug)");

                        // if (_dbgCallEnterDialogue)
                        // {
                        // }
                        // else Debug.Log("[DialogueTrigger] EnterDialogueMode skipped (debug)");
                    }
                }
            }
        }
        else
        {
            _interactUI.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {   
            _playerInRange = true;
            Debug.Log("PLAYER ENTERED "+ this.transform.parent.GetComponent<NPCScript>().GetNPCName()  +" TRIGGER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerInRange = false;
            Debug.Log("PLAYER EXITED "+ this.transform.parent.GetComponent<NPCScript>().GetNPCName() +" TRIGGER");
        }
    }

    private void HideUI(CanvasGroup cg)
    {
        cg.alpha = 0f;
    }

    private void ShowUI(CanvasGroup cg)
    {
        cg.alpha = 1f;
    }
}