using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    Vector3 change;
    Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        change = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  target.position - change;
    }
}
