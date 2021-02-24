using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{   
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
        Vector3 nodePos = PathRequestManager.GetGrid().WorldToNode(mousePos).worldPosition;
        Vector3 oldPos = Camera.main.ScreenToWorldPoint(controls.Keyboard.MousePos.ReadValue<Vector2>());

        if (oldPos != new Vector3 (nodePos.x,nodePos.y,1) || LClick)
        {
            PathRequestManager.GetGrid().WorldToNode(transform.position).SetOccupied(null);
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
            }
            else
            {
                GetComponent<PathFindingUnit>().StopPath();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Exit"))
        {
            if (SceneManager.GetActiveScene().name == "CampFire")
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.FadeOut("Campfire Theme");
                    AudioManager.instance.FadeIn("Overworld Theme");
                }
                ScreenManager.instance.SwitchLevel("Final Base");
            }
            else if (SceneManager.GetActiveScene().name == "Final Base")
            {
                int direction = collision.transform.tag.ToString()[4] - 48;

                FindObjectOfType<MapGeneration>().StartSwap(direction);

            }
        }
    }
}
