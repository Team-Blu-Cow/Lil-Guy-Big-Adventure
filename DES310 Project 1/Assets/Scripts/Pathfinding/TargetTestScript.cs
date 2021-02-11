using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTestScript : MonoBehaviour
{
    [HideInInspector] public Vector3 mousePos;
    InputMaster input;
    public IsoGrid grid;

    private void Awake()
    {
        input = new InputMaster();
        input.PathfinderTestControls.MousePos.performed += ctx => SetCursorPosition(ctx.ReadValue<Vector2>());
        input.PathfinderTestControls.MouseClick.started += ctx => SetTargetPosition();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void SetCursorPosition(Vector2 pos)
    {
        Vector2 worldPos;
        worldPos = Camera.main.ScreenToWorldPoint(pos);
        mousePos = new Vector3(worldPos.x, worldPos.y, 1);
    }

    void SetTargetPosition()
    {
        Vector3 nodePos = grid.WorldToNode(mousePos).worldPosition;
        transform.position = new Vector3(nodePos.x, nodePos.y, 1);
    }
}
