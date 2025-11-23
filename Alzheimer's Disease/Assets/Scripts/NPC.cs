using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Details")]
    [SerializeField] private string _npcName;
    
    private GameEventsManager _gameEventsManager;

    private bool _hasInteracted = false;

    // collider?

    private void Start()
    {
        _gameEventsManager = GameEventsManager.Instance;
        _gameEventsManager.npcEvents.onNPCInteract += OnInteract;
    }

    private void OnDestroy()
    {
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.onNPCInteract -= OnInteract;
    }

    public void Interact()
    {
        // Debug.Log("NPC INTERACTED (sent from NPC.cs)"); 
        // GameEventsManager.Instance.npcEvents.NPCInteracted();
        // do something
    }

    public void OnInteract()
    {   
        if (_hasInteracted) return;
        // mark as interacted before broadcasting to avoid re-entrant recursion
        _hasInteracted = true;
        Debug.Log("NPC ONINTERACT TRIGGERED (sent from NPC.cs)");
        if (_gameEventsManager != null)
            _gameEventsManager.npcEvents.NPCInteracted();
    }   
}
