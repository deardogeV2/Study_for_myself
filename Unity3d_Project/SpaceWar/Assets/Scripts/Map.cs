using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    MeshRenderer render;
    float speed = 0.1f;
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        render.material.mainTextureOffset = new Vector2(0, Time.time) * speed;
    }
}
