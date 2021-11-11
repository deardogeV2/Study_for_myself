using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    Rigidbody rig;//Ball身上的碰撞控件

    public float speed = 3; // 定义速度参数，同时可以在unity里面看到
    public int all_scorce = 0;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>(); // 获取Rigidbody组件
    }

    // Update is called once per frame
    void Update()
    {
        
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //Debug.Log("水平："+h+"垂直："+v);

        //rig.AddForce(new Vector3(h, 0, v) * speed );
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rig.AddForce(new Vector3(h, 0, v) * speed * 5);
    }

    public void OnAddScore(int cur_score)
    {
        all_scorce += cur_score;
        Debug.Log("现在人物有：" + all_scorce+ "分");
    }
}
