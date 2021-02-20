using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgPopupTestScript : MonoBehaviour
{
    void Start()
    {
        DamagePopup.Create(Vector3.one*-1, 300);
    }

    public void CreatePopup(Vector3 pos)
    {
        bool is_crit = Random.Range(0,10) < 1;
        DamagePopup.Create(pos, 300, is_crit);
    }

}
