using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    public NPCEventBus npcEvents;
    public QuestEvents questEvents;
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public DialogueEvents dialogueEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameEventsManager found in scene");
        }
        Instance = this;

        npcEvents = new NPCEventBus();
        questEvents = new QuestEvents();
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        dialogueEvents = new DialogueEvents();

        DontDestroyOnLoad(this.gameObject);
    }
}
