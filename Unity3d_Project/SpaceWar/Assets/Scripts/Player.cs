using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
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
            //生成子弹对象，并且初始化子弹的位置在发射点的位置上。
            GameObject goBullet = Instantiate<GameObject>(prefab_bullet);
            goBullet.transform.position = fire_point.position;
        }
    }
}
