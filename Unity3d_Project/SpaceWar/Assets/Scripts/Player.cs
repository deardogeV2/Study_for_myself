using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody rig;
    float speed = 3f;

    float minX = -3.9f;
    float maxX = 3.9f;

    float minZ = -5.1f;
    float maxZ = 5.1f;

    GameObject prefab_bullet;
    Transform fire_point;

    void Start()
    {
      rig = GetComponent<Rigidbody>();

        fire_point = transform.Find("FirePoint");
        prefab_bullet = Resources.Load<GameObject>("Bullet");
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

        if (h == 0f)
        {
            if (transform.rotation.z > 0)
            {
                rig.angularVelocity = new Vector3(0, 0, -30) * 1f;
            }
            else if (transform.rotation.z < 0)
            {
                rig.angularVelocity = new Vector3(0, 0, 30) * 1f;
            }
        }

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 生成子弹对象
            GameObject goBullet = Instantiate<GameObject>(prefab_bullet);
            //将子弹对象的位置设置于开火点。（可以思考一下这样是否会有很短的一瞬间，子弹会在0，0，0位置与别的物体发生碰撞？）
            //碰撞条件为两物体都有collider 碰撞体组件。
            goBullet.transform.position = fire_point.position;
        }
    }
}
