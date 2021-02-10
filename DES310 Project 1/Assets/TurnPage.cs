using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPage : MonoBehaviour
{

    public GameObject leftPage;
    public GameObject rightPage;
    public GameObject newLeftPage;
    public GameObject newRightPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipPageForward()
    {
        newLeftPage.transform.localScale = new Vector3(0,1,1);
        rightPage.transform.localScale = new Vector3(1,1,1);

        LeanTween.cancelAll();
        LeanTween.scaleX(rightPage, 0, 0.5f);
        LeanTween.delayedCall(0.5f, RightStart);
    }
    
    public void FlipPageBackward()
    {
        rightPage.transform.localScale = new Vector3(0,1,1);
        newLeftPage.transform.localScale = new Vector3(1,1,1);

        LeanTween.cancelAll();
        LeanTween.scaleX(newLeftPage, 0, 0.5f);
        LeanTween.delayedCall(0.5f, LeftStart);
    }

    void RightStart()
    {
        LeanTween.scaleX(newLeftPage, 1, 0.5f);
    }
    
    void LeftStart()
    {
        LeanTween.scaleX(rightPage, 1, 0.5f);
    }
}
