using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

/*[CustomEditor(typeof(GridHighLighter))]
public class GridHighLighterEditor : Editor
{
    bool showTileSettings, showOverlayColours, showDebugSettings, showPathDawingSettings = false;

    SerializedProperty overlayTileMap;
    SerializedProperty worldTileMap;
    SerializedProperty grid;
    SerializedProperty highlightTile;

    SerializedProperty baseColor;
    SerializedProperty unwalkableColor;
    SerializedProperty selectableColor;

    SerializedProperty test;
    SerializedProperty testMoveSpeed;

    SerializedProperty sprites;
    SerializedProperty path;
    SerializedProperty pathNodePrefab;

    private void OnEnable()
    {
        overlayTileMap      = serializedObject.FindProperty("overlayTileMap");
        worldTileMap        = serializedObject.FindProperty("worldTileMap");
        grid                = serializedObject.FindProperty("grid");
        highlightTile       = serializedObject.FindProperty("highlightTile");

        baseColor           = serializedObject.FindProperty("baseColor");
        unwalkableColor     = serializedObject.FindProperty("unwalkableColor");
        selectableColor     = serializedObject.FindProperty("selectableColor");

        test                = serializedObject.FindProperty("test");
        testMoveSpeed       = serializedObject.FindProperty("testMoveSpeed");

        sprites             = serializedObject.FindProperty("sprites");
        path                = serializedObject.FindProperty("path");
        pathNodePrefab      = serializedObject.FindProperty("pathNodePrefab");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        showTileSettings = EditorGUILayout.Foldout(showTileSettings, "Tile/Grid Data");
        if (showTileSettings)
        {
            EditorGUILayout.PropertyField(overlayTileMap);
            EditorGUILayout.PropertyField(worldTileMap);
            EditorGUILayout.PropertyField(grid);
            EditorGUILayout.PropertyField(highlightTile);
            EditorGUILayout.Space(10);
        }

        showOverlayColours = EditorGUILayout.Foldout(showOverlayColours, "Overlay Colours");
        if (showOverlayColours)
        {
            EditorGUILayout.PropertyField(baseColor);
            EditorGUILayout.PropertyField(unwalkableColor);
            EditorGUILayout.PropertyField(selectableColor);
            EditorGUILayout.Space(10);
        }

        showDebugSettings = EditorGUILayout.Foldout(showDebugSettings, "Debug Settings");
        if (showDebugSettings)
        {
            EditorGUILayout.PropertyField(test);
            EditorGUILayout.PropertyField(testMoveSpeed);
            EditorGUILayout.Space(10);
        }

        showPathDawingSettings = EditorGUILayout.Foldout(showPathDawingSettings, "Path Drawing Data");
        if (showPathDawingSettings)
        {
            EditorGUILayout.PropertyField(sprites);
            EditorGUILayout.PropertyField(path);
            EditorGUILayout.PropertyField(pathNodePrefab);
            EditorGUILayout.Space(10);
        }
    }
}*/
