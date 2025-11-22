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
                if (Physics.Raycast(r, out RaycastHit hitInfo, _interactRange))
                {
                    // DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
                    NPC npc = hitInfo.collider.GetComponentInParent<NPC>();
                    if (npc != null)
                    {
                        GameEventsManager.instance.npcEvents.NPCInteracted();
                        npc.Interact();
                        DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
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
            Debug.Log("PLAYER ENTERED NPC TRIGGER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerInRange = false;
            Debug.Log("PLAYER EXITED NPC TRIGGER");
        }
    }
}
