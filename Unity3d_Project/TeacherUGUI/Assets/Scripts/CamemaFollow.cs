using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamemaFollow : MonoBehaviour 
{
    Vector3 offset;//目标指向摄像机的相对向量
    Transform transPlayer;//摄像机跟随的目标
    public float minX, maxX;
    public float minZ, maxZ;
    public bool isClamp;
    void Start ()
	{
        transPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - transPlayer.position;
        
    }
	
	void Update () 
	{
        //更新摄像机的位置
        transform.position = transPlayer.position + offset;
        if(isClamp)
        {
            Vector3 oldPos = transform.position;
            oldPos.x = Mathf.Clamp(oldPos.x, minX, maxX);
            oldPos.z = Mathf.Clamp(oldPos.z, minZ, maxZ);
            transform.position = oldPos;
        }
       
    }
}
