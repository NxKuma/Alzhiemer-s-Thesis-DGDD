using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Details")]
    [SerializeField] private string _npcName;

    // collider?

    public void Interact()
    {
        Debug.Log("NPC INTERACTED (sent from NPC.cs)");
        // do something
    }
}
