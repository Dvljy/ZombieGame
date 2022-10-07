using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Action sequnese;

    void Start()
    {
        sequnese += One;
        sequnese += Two;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sequnese();
        }
    }

    void One()
    {
        Debug.Log(1);
    }

    void Two()
    {
        Debug.Log(2);
    }
}
