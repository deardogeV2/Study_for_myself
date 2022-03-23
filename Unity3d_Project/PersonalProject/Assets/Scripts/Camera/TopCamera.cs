using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCamera : MonoBehaviour
{
    Vector3 change;
    Camera cameraThis;
    public Transform targetTsf;
    void Start()
    {
        cameraThis = GetComponent<Camera>();
        Transform targetTsf = GameObject.FindGameObjectWithTag("Player").transform;
        change = transform.position - targetTsf.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetTsf.position+ change;
        transform.LookAt(targetTsf.position);
    }
}
