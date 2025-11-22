using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public NPCEventBus npcEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameEventsManager found in scene");
        }
        instance = this;

        npcEvents = new NPCEventBus();
    }
}
