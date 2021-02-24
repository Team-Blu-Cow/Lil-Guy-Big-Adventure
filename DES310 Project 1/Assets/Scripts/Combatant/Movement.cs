using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public IsoGrid grid;
    InputManager controls;

    bool RMouseDown = false;
    bool LClick = false;
    float timer;

    private void Awake()
    {
        controls = new InputManager();

        controls.Keyboard.RClick.started += ctx => { RMouseDown = true; };
        controls.Keyboard.RClick.canceled += ctx => { RMouseDown = false; };
        controls.Keyboard.LClick.performed += ctx => LeftDown();        
    }

    private void Start()
    {
        grid = PathRequestManager.GetGrid();//FindObjectOfType<IsoGrid>();
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(controls.Keyboard.MousePos.ReadValue<Vector2>());
        Vector3 nodePos = grid.WorldToNode(mousePos).worldPosition;
        Vector3 oldPos = Camera.main.ScreenToWorldPoint(controls.Keyboard.MousePos.ReadValue<Vector2>());

        if (oldPos != new Vector3 (nodePos.x,nodePos.y,1) || LClick)
        {
            grid.WorldToNode(transform.position).SetOccupied(null);
            GetComponent<PathFindingUnit>().StartPath();
            LClick = false;
        }
    }

    private void LeftDown()
    {
        LClick = true;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(controls.Keyboard.MousePos.ReadValue<Vector2>());
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit)
            {
                Item item;
                if (hit.collider.TryGetComponent<Item>(out item))
                {
                    item.PickUp(transform.position);
                }
                else if (hit.collider.gameObject.transform.tag.Contains("Exit"))
                {                                      
                    IsoNode node = grid.WorldToNode(hit.collider.gameObject.transform.position);

                    int i = hit.collider.gameObject.transform.tag.ToString()[4] - 48;

                    foreach (IsoNode neighbor in grid.GetNeighbors(node))
                    {
                        if (neighbor != null)
                        {
                            if (neighbor.gridPosition == grid.WorldToNode(transform.position).gridPosition) //if it is a treasure
                            {
                                FindObjectOfType<MapGeneration>().StartSwap(i);
                                grid.CreateGrid();
                            }
                        }
                    }

                    if (node.gridPosition == grid.WorldToNode(transform.position).gridPosition)
                    {
                        FindObjectOfType<MapGeneration>().StartSwap(i);
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
}
