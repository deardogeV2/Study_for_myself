using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyShip : MonoBehaviour
{
    // 子弹发射参数
    GameObject prefab;
    public float fire_gap = 1f;
    private float last_fire_time = 0f;
    Transform transform_fire_point;

    // 飞机移动参数
    public float speed = 1f;
    public Vector3 dir = Vector3.back;
    Rigidbody rig;
    void Start()
    {
        prefab = Resources.Load<GameObject>("EnemyBullet");
        transform_fire_point = transform.Find("FirePoint");
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = dir * speed;

        if (Time.time - last_fire_time > fire_gap)
        {
            GameObject newEnemyBullet = Instantiate<GameObject>(prefab);
            newEnemyBullet.transform.parent = this.transform;
            newEnemyBullet.transform.position = transform_fire_point.position;

            last_fire_time = Time.time;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PlayerBullet" || other.tag == "Player")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
