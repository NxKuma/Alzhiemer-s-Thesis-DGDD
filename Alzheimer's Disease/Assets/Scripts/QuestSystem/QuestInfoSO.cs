using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }

    [Header("General")]
    public string DisplayName;

    [Header("Requirements")]
    public int PhaseRequirement;
    public Item[] ItemRequirements;
    public QuestInfoSO[] QuestPrerequesites;

    [Header("Quest Steps")]
    public GameObject[] QuestStepPrefabs;

    // [Header("Rewards")]
    
    // ensure the id is the name of the Scriptable Object asset
    private void OnValidate()
    {
        #if UNITY_EDITOR
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
