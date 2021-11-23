using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Vector3 dir = Vector3.forward;
    public float speed = 3f;
    void Start()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        rig.velocity = dir * speed;
    }
}
