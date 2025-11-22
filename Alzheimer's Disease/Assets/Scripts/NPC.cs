using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Details")]
    [SerializeField] private string _npcName;

    // collider?

    private void Start()
    {
        GameEventsManager.instance.npcEvents.onNPCInteract += OnInteract;
    }

    private void OnDestroy()
    {
        if (GameEventsManager.instance != null)
            GameEventsManager.instance.npcEvents.onNPCInteract -= OnInteract;
    }

    public void Interact()
    {
        // Debug.Log("NPC INTERACTED (sent from NPC.cs)"); 
        // GameEventsManager.instance.npcEvents.NPCInteracted();
        // do something
    }

    public void OnInteract()
    {
        Debug.Log("NPC ONINTERACT TRIGGERED (sent from NPC.cs)");
        GameEventsManager.instance.npcEvents.NPCInteracted();
    }
}
