using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodControl : MonoBehaviour
{
    public float speed = 3;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        score = Random.Range(10, 50);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision) // 碰撞触发触发函数。控件选择了 is Tragger后无法触发
    {
        DestroyObject(this.gameObject);//删除游戏组件, this 是指这个脚本控件。而this.gameObject可以获取控件所在游戏组件的对象。
    }

    private void OnTriggerEnter(Collider other) // 触发器触发函数。
    {
        if (other.tag == "Player")
        {
            //获取人物对象
            //GameObject player = GameObject.Find("Ball"); 不推荐

            //通过标签获取人物对象,这种方式也是属于直接通信，可以解耦。
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            //if (player)
            //{
            //    BallControl ball_control = player.GetComponent<BallControl>();
            //    ball_control.all_source += source;
            //    Debug.Log("现在人物有：" + ball_control.all_source);
            //}

            //通过消息的方式进行通信
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                player.SendMessage("OnAddScore", score);


                //直接获取上层对象的通信方式，不是很好。
                //GameObject go = GameObject.Find("FoodManager");
                //FoodManagerControl fmc = go.GetComponent<FoodManagerControl>();
                //fmc.deleteFood(this.gameObject);//删除游戏组件, this 是指这个脚本控件。而this.gameObject可以获取控件所在游戏组件的对象。

                //通过消息的方式与FoodManage进行通信
                SendMessageUpwards("deleteFood", this.gameObject);

            }
        }
    }
}
