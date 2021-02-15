using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public IsoGrid grid;
    InputManager controls; 
    Transform target;

    bool RMouseDown = false;
    bool LClick = false;
    float timer;

    private void Awake()
    {
        target = GetComponent<PathFindingUnit>().target;

        controls = new InputManager();
        //controls.Keyboard.RClick.started += ctx => { RMouseDown = true; };
        //controls.Keyboard.RClick.canceled += ctx => { RMouseDown = false; };
        //controls.Keyboard.LClick.performed += ctx => Stop();
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
        /*Vector2 mousePos = Camera.main.ScreenToWorldPoint(controls.Keyboard.MousePos.ReadValue<Vector2>());
        Vector3 nodePos = grid.WorldToNode(mousePos).worldPosition;
        Vector3 oldPos = target.position;
        target.position = new Vector3(nodePos.x, nodePos.y, 1);

        if (oldPos != new Vector3 (nodePos.x,nodePos.y,1) || LClick)
        {
            GetComponent<PathFindingUnit>().StartPath();
            LClick = false;
        }*/
    }

    private void Stop()
    {
        LClick = true;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(controls.Keyboard.MousePos.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
       
        if (hit)
        {
            Item item;
            if (hit.collider.TryGetComponent<Item>(out item))
            {                
                item.PickUp(transform.position);
            }
            else if (hit.collider.gameObject.CompareTag("Exit"))
            {
                IsoNode node = grid.WorldToNode(hit.collider.gameObject.transform.position);

                foreach (IsoNode neighbor in grid.GetNeighbors(node))
                {
                    if (neighbor.gridPosition == grid.WorldToNode(transform.position).gridPosition) //if it is a treasure
                    {
                        FindObjectOfType<MapGeneration>().RenderMap();
                        grid.CreateGrid();
                    }
                }
                if (node.gridPosition == grid.WorldToNode(transform.position).gridPosition)
                {
                    FindObjectOfType<MapGeneration>().RenderMap();
                    grid.CreateGrid();
                }
            }
        }
        else
        {
            GetComponent<PathFindingUnit>().StopPath();
        }
}
}
