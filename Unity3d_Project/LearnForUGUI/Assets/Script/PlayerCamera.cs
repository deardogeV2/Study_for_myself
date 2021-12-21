using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //偏移量
    Vector3 offset;
    //摄像机跟踪对象
    Transform TsfPlayer;
    float minX, maxX;
    float minZ, maxZ;
    void Start()
    {

        TsfPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        //计算offset
        offset = transform.position - TsfPlayer.position;

        //简单给出边界
        minX = -20f;
        maxX = 20f;
        minZ = -20f;
        maxZ = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = TsfPlayer.position + offset;
        Vector3 oldPosition = transform.position;
        oldPosition.x = Mathf.Clamp(oldPosition.x, minX, maxX);
        oldPosition.z = Mathf.Clamp(oldPosition.z, minZ, maxZ);
        transform.position = oldPosition;
    }
}
