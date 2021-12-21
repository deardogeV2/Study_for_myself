using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForHeadPortrait : MonoBehaviour
{
    //ƫ����
    Vector3 offset;
    //��������ٶ���
    Transform TsfPlayer;
    void Start()
    {

        TsfPlayer = GameObject.FindGameObjectWithTag("PlayerUI").transform;
        //����offset
        offset = transform.position - TsfPlayer.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = TsfPlayer.position + offset;
    }
}
