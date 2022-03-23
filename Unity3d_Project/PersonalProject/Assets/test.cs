using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Transform TestBottom;
    void Start()
    {
        TestBottom = transform.Find("TestBottom").transform;

        transform.RotateAround(TestBottom.position, transform.forward, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
