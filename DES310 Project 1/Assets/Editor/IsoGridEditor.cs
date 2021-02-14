using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(IsoGrid))]
public class IsoGridEditor : Editor
{
    bool showGridSettings, showNodeSettings, showDebugOptions = false;

    SerializedProperty gridSize;
    SerializedProperty tileMap;
    SerializedProperty highlighter;

    SerializedProperty nodeRadius;
    SerializedProperty tileData;

    SerializedProperty DisplayGridGizmos;
    SerializedProperty GridGizmoOpacity;
    SerializedProperty DisplayNodeGizmos;

    private void OnEnable()
    {
        gridSize            = serializedObject.FindProperty("gridSize");
        tileMap             = serializedObject.FindProperty("tileMap");
        highlighter         = serializedObject.FindProperty("highlighter");

        nodeRadius          = serializedObject.FindProperty("nodeRadius");
        tileData            = serializedObject.FindProperty("tileData");

        DisplayGridGizmos   = serializedObject.FindProperty("DisplayGridGizmos");
        GridGizmoOpacity    = serializedObject.FindProperty("GridGizmoOpacity");
        DisplayNodeGizmos   = serializedObject.FindProperty("DisplayNodeGizmos");

        IsoGrid grid = (IsoGrid)target;
        grid.InitGrid();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        showGridSettings = EditorGUILayout.Foldout(showGridSettings, "Grid Settings");
        if (showGridSettings)
        {
            EditorGUILayout.PropertyField(gridSize);
            EditorGUILayout.PropertyField(tileMap);
            EditorGUILayout.PropertyField(highlighter);
            EditorGUILayout.Space(10);
        }

        showNodeSettings = EditorGUILayout.Foldout(showNodeSettings, "Node Settings");
        if (showNodeSettings)
        {
            EditorGUILayout.PropertyField(nodeRadius);
            EditorGUILayout.PropertyField(tileData);
            EditorGUILayout.Space(10);
        }

        showDebugOptions = EditorGUILayout.Foldout(showDebugOptions, "Debug Settings");
        if (showDebugOptions)
        {
            EditorGUILayout.PropertyField(DisplayGridGizmos);
            EditorGUILayout.PropertyField(GridGizmoOpacity);
            EditorGUILayout.PropertyField(DisplayNodeGizmos);
            EditorGUILayout.Space(10);
        }

        serializedObject.ApplyModifiedProperties();

    }

    
}
