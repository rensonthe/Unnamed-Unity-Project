using UnityEngine;
using System.Collections;

public class ChangeAlpha : MonoBehaviour {

    public float alphaLevel = 1;
    public float totalTime = 0;

    void Update()
    {
        totalTime += Time.deltaTime;

        if (totalTime >= .09)
        {
            alphaLevel -= .01f;
            totalTime = 0;
        }

        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alphaLevel);
    }
}
