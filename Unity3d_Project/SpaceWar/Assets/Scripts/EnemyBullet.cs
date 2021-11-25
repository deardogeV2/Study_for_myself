using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    Rigidbody rig;
    public float speed = 8f;
    public Vector3 dir = Vector3.back;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;

        rig.velocity = dir * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
