using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantAnimations : MonoBehaviour
{
    private Animator animator;
    private Transform transform;

    private Vector3 last;

    bool lock_rotation = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    private void Update()
    {

        if(lock_rotation == false)
        {
            if (last == null)
            {
                last = transform.position;
                return;
            }

            Vector3 diff = last - transform.position;
            if (diff.y < 0)
                animator.SetBool("FacingForwards", false);
            else
                animator.SetBool("FacingForwards", true);

            if (diff.x < 0)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
                transform.localRotation = Quaternion.Euler(0, 0, 0);


            if (last != transform.position)
                animator.SetBool("IsMoving", true);
            else
                animator.SetBool("IsMoving", false);

            last = transform.position;
        }

    }

    public IEnumerator LockRotationForAttack(GameObject target)
    {
        lock_rotation = true;

        Vector3 diff = transform.position - target.GetComponent<Transform>().position;
        if (diff.y < 0)
            animator.SetBool("FacingForwards", false);
        else
            animator.SetBool("FacingForwards", true);

        if (diff.x < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(2f);

        lock_rotation = false;
    }







}
