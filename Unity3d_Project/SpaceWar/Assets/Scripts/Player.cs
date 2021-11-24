using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody rig;
    float speed = 5f;

    float minX = -3.9f;
    float maxX = 3.9f;

    float minZ = -5.1f;
    float maxZ = 5.1f;

    private float last_fire_time;

    GameObject prefab_bullet;
    Transform fire_point;

    private float fire_gap = 0.5f;

    void Start()
    {
      rig = GetComponent<Rigidbody>();

        fire_point = transform.Find("FirePoint");
        prefab_bullet = Resources.Load<GameObject>("Bullet");
        last_fire_time = Time.time;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (rig)
        {
            rig.velocity = new Vector3(h, 0, v) * speed;
            // 角度旋转方案之一，使用欧拉角直接进行旋转。new Vector3(0,0,-30)为常向量，h为水平轴取值（-1~1），直接相乘可以实现我们希望的左右按键水平移动的时候飞机左右在一定角度上发生旋转的方案。
            transform.eulerAngles = new Vector3(0, 0, -30) * h;
        }

        

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;


    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > last_fire_time + fire_gap)// 简单的发射间隔逻辑
            {
                // 生成子弹对象
                GameObject goBullet = Instantiate<GameObject>(prefab_bullet);
                //将子弹对象的位置设置于开火点。（可以思考一下这样是否会有很短的一瞬间，子弹会在0，0，0位置与别的物体发生碰撞？）
                //碰撞条件为两物体都有collider 碰撞体组件。
                goBullet.transform.position = fire_point.position;

                //更新最后开火时间
                last_fire_time = Time.time;
            }
        }
    }
}
