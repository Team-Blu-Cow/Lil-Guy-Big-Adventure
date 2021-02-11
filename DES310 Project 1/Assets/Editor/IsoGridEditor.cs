using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(IsoGrid))]
public class IsoGridEditor : Editor
{
    private void OnEnable ()
    {
        IsoGrid grid = (IsoGrid)target;

        grid.InitGrid();
    }

    private void OnValidate()
    {
        IsoGrid grid = (IsoGrid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IsoGrid grid = (IsoGrid)target;

        if (GUILayout.Button("Generate Nodes"))
        {
            grid.CreateGrid();
        }
    }

    
}
