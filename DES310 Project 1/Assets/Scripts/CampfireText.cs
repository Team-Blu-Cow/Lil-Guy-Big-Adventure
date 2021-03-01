using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireText : MonoBehaviour
{
    public float time = 0.0f;
    public float multiplier = 0.15f;
    public float speed = 10f;
    private float initYPos;
    void Start()
    {
        initYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime*speed;

        float y_pos = initYPos+(Mathf.Sin(time)*multiplier);

        transform.position = new Vector3(transform.position.x, y_pos, transform.position.z);
    }
}
