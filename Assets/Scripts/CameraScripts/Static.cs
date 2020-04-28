using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Turns static off after a short time to conceal threat movements
public class Static : MonoBehaviour
{
    public float StaticTime = 0.5f;
    public float TimeRemaining;


    void Start()
    {
        TimeRemaining = StaticTime;
    }

    void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0)
        {
            TimeRemaining = StaticTime;
            gameObject.SetActive(false);
        }
    }
}