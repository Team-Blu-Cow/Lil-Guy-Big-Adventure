using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharAnimations : MonoBehaviour
{
    private Animator animator;
    private Transform transform;


    private Vector3 last;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (last == null)
        {
            last = transform.position;
            return;
        }

        Vector3 diff = last - transform.position;
        if (diff.y < 0)
        {
            animator.SetBool("FacingForwards", false);
        }
        else
        {
            animator.SetBool("FacingForwards", true);
        }


        Vector3 scale = transform.localScale;



        if(diff.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }







        if (last != transform.position)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        last = transform.position;




    }
}
