using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //ƫ����
    Vector3 offset;
    //��������ٶ���
    Transform TsfPlayer;
    float minX, maxX;
    float minZ, maxZ;
    void Start()
    {

        TsfPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        //����offset
        offset = transform.position - TsfPlayer.position;

        //�򵥸����߽�
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
