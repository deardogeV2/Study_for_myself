using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    Rigidbody rig;//Ball身上的碰撞控件

    public float speed = 3; // 定义速度参数，同时可以在unity里面看到
    public int all_scorce = 0;
    private bool is_alive = true;
    private float TimeStep = 0;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(Vector3.up * 500);
        }

        rig.AddForce(new Vector3(h, 0, v) * speed * 5);
    }

    public void OnAddScore(int cur_score)
    {
        all_scorce += cur_score;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 400, 200), "现在人物有：" + all_scorce + "分");
        TimeCheck();

        if (!is_alive)
        {
            GUI.Label(new Rect((Screen.width >> 1) - 100, (Screen.height >> 1) - 100, 200, 200), "GameOver!");
            if (GUI.Button(new Rect((Screen.width >> 1) - 100, (Screen.height >> 1), 200, 200), "Restart!"))
            {
                //重新开始游戏，重新加载场景
                Application.LoadLevel("SampleScene");
                
            }
        }
    }

    public void TimeCheck()
    {
        TimeStep += Time.deltaTime;
        GUI.Label(new Rect(Screen.width >> 1, 100, 200, 200), "Time: " + TimeStep);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dead_area"))
        {
            is_alive = false;
        }
    }
}
