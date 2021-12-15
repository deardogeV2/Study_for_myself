using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForHeadPortrait : MonoBehaviour
{
    //偏移量
    Vector3 offset;
    //摄像机跟踪对象
    Transform TsfPlayer;
    void Start()
    {

        TsfPlayer = GameObject.FindGameObjectWithTag("PlayerUI").transform;
        //计算offset
        offset = transform.position - TsfPlayer.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = TsfPlayer.position + offset;
    }
}
