using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    public NPCEventBus npcEvents;
    public QuestEventBus questEvents;
    // public InputEventBus inputEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameEventsManager found in scene");
        }
        Instance = this;

        npcEvents = new NPCEventBus();
        questEvents = new QuestEventBus();
    }
}
