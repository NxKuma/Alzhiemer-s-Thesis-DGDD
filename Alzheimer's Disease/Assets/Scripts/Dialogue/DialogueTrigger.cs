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
                Debug.DrawRay(r.origin, r.direction * _interactRange, Color.red, 2.0f);
                int mask = LayerMask.GetMask("NPC");

                // Debug.Log(r.hitInfo.collider.gameObject.name);
                if (Physics.Raycast(r, out RaycastHit hitInfo, _interactRange, mask))
                {
                    DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
                    // if (hitInfo.collider.gameObject.TryGetComponent(out NPCInteractable npc))
                    // {
                    //     // npc.Interact();
                    //     Debug.Log(_inkJSON.text);
                    // }
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
