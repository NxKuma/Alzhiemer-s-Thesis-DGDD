using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlacementManager))]
public class PlacementManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlacementManager placementManager = (PlacementManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Placement Tools", EditorStyles.boldLabel);

        if (GUILayout.Button($"Save Placements ({placementManager.GetEditorTargetPhase()})"))
        {
            Undo.RecordObject(placementManager, "Save Phase Placements");
            placementManager.SavePlacementsForEditorPhase();
            EditorUtility.SetDirty(placementManager);
        }
    }
}
