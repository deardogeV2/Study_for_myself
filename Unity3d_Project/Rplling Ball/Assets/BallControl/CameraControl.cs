using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 distance;
    public Transform ball_transform;
    void Start()
    {
        GameObject PlayerBall = GameObject.FindGameObjectWithTag("Player");
        ball_transform = PlayerBall.transform;
        distance = transform.position - ball_transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ball_transform.position + distance;
    }
}
