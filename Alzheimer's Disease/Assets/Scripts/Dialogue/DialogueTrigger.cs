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

    [Header("Debug - isolate leak")]
    [Tooltip("If false, the Physics.Raycast call will be skipped (no hit).")]
    [SerializeField] private bool _dbgAllowRaycast = true;
    [Tooltip("If false, GameEventsManager.npcEvents.NPCInteracted() won't be called.")]
    [SerializeField] private bool _dbgCallEvent = true;
    [Tooltip("If false, npc.Interact() won't be called.")]
    [SerializeField] private bool _dbgCallNpcInteract = true;
    [Tooltip("If false, DialogueManager.EnterDialogueMode won't be called.")]
    [SerializeField] private bool _dbgCallEnterDialogue = true;

    private void Awake()
    {
        _playerInRange = false;
        _interactUI.SetActive(false);
    }

    // private void Start()
    // {
    //     _interactorSource = GameObject.FindWithTag("Player").transform;
    //     _interactUI = GameObject.FindWithTag("InteractUI");
    // }

    private void Update()
    {
        if (_playerInRange && !DialogueManager.GetInstance().DialogueIsPlaying)
        {
            _interactUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray r = new Ray(_interactorSource.position, _interactorSource.forward);
                Debug.DrawRay(r.origin, r.direction * _interactRange, Color.red, 2.0f);
                int mask = LayerMask.GetMask("NPC");

                if (!_dbgAllowRaycast)
                {
                    Debug.Log("[DialogueTrigger] Raycast skipped (debug)");
                }
                else if (Physics.Raycast(r, out RaycastHit hitInfo, _interactRange, mask))
                {
                    NPCScript npc = hitInfo.collider.GetComponentInParent<NPCScript>();
                    if (npc != null)
                    {
                        if (_dbgCallEvent)
                        {
                            GameEventsManager.Instance.npcEvents.NPCInteracted();
                        }
                        else Debug.Log("[DialogueTrigger] NPCInteracted skipped (debug)");

                        if (_dbgCallNpcInteract)
                        {   
                            npc.Interact();
                        }
                        else Debug.Log("[DialogueTrigger] npc.Interact skipped (debug)");

                        if (_dbgCallEnterDialogue)
                        {
                            DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
                        }
                        else Debug.Log("[DialogueTrigger] EnterDialogueMode skipped (debug)");
                    }
                }
            }
        }
        else
            _interactUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerInRange = true;
            Debug.Log("PLAYER ENTERED"+ this.transform.parent.name  +" TRIGGER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerInRange = false;
            Debug.Log("PLAYER EXITED"+ this.transform.parent.name  +" TRIGGER");
        }
    }
}
