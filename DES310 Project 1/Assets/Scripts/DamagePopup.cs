using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 pos, int dmg, bool isCrit = false, bool isHeal = false)
    {
        Transform pfDamagePopupObj = Instantiate(Resources.Load<Transform>("Prefabs/pfDamagePopup"), pos, Quaternion.identity);
        pfDamagePopupObj.GetComponent<TMPro.TextMeshPro>().sortingOrder = 1;

        DamagePopup dmgPopup = pfDamagePopupObj.GetComponent<DamagePopup>();
        dmgPopup.Setup(dmg, isCrit, isHeal);

        return dmgPopup;
    }

    public static DamagePopup Create(string text, Vector3 pos)
    {
        Transform pfDamagePopupObj = Instantiate(Resources.Load<Transform>("Prefabs/pfDamagePopup"), pos, Quaternion.identity);
        pfDamagePopupObj.GetComponent<TMPro.TextMeshPro>().sortingOrder = 1;

        DamagePopup dmgPopup = pfDamagePopupObj.GetComponent<DamagePopup>();
        dmgPopup.Setup(text);

        return dmgPopup;
    }

    [SerializeField] private const float disappearTimerMax = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer = 1f;
    private Color textColor;
    private Vector3 moveVector;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color criticalColor;
    [SerializeField] private Color healColor;
    [SerializeField] private Color criticalHealColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageNum, bool isCrit = false, bool isHeal = false)
    {
        textMesh.SetText(damageNum.ToString());
        if (!isCrit)
        {
            textMesh.fontSize = 4;
            if(isHeal)
                textColor = healColor;
            else
                textColor = normalColor;
        }
        else
        {
            textMesh.fontSize = 5;
            if (isHeal)
                textColor = criticalHealColor;
            else
                textColor = criticalColor;

        }
        textMesh.color = textColor;
        disappearTimer = disappearTimerMax;
        moveVector = new Vector3(0, 1) * 4f;
    }

    public void Setup(string text)
    {
        textMesh.SetText(text);
        textMesh.fontSize = 4;
        textMesh.color = Color.white;
        disappearTimer = disappearTimerMax;
        moveVector = new Vector3(0, 1) * 4f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 5f * Time.deltaTime;

        if (disappearTimer > disappearTimerMax*0.75f)
        {
            float increaseScaleFactor = 1.8f;
            transform.localScale += Vector3.one * increaseScaleFactor * Time.deltaTime;
        }
        else
        {
            float decreaseScaleFactor = 0.8f;
            transform.localScale -= Vector3.one * decreaseScaleFactor * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer<0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
