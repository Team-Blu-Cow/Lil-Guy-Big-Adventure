using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    InputManager controls; 
    public AStarGrid grid;
    Transform target;

    bool RMouseDown;
    float timer;

    private void Awake()
    {
        target = GetComponent<PathFindingUnit>().target;

        controls = new InputManager();
        controls.Keyboard.RClick.started += ctx => { RMouseDown = true; };
        controls.Keyboard.RClick.canceled += ctx => { RMouseDown = false; };
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (RMouseDown && timer > 0.2)
        {
            Move();
            timer = 0;
        }
    }

    void Move()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(controls.Keyboard.Mouse.ReadValue<Vector2>());
        Vector3 nodePos = grid.WorldToNode(mousePos).worldPosition;
        Vector3 oldPos = target.position;
        target.position = new Vector3(nodePos.x, nodePos.y, 1);

        if (oldPos != new Vector3 (nodePos.x,nodePos.y,1))
        {
            GetComponent<PathFindingUnit>().StartPath();
        }
    }
}
