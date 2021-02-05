using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(AStarGrid))]
public class AStarGridEditor : Editor
{
    private void OnEnable ()
    {
        AStarGrid grid = (AStarGrid)target;

        grid.InitGrid();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AStarGrid grid = (AStarGrid)target;

        if (GUILayout.Button("Generate Nodes"))
        {
            grid.CreateGrid();
        }
    }

    
}
