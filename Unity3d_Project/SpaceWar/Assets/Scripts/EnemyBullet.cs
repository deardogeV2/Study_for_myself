using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    Rigidbody rig;
    public float speed = 8f;
    public Vector3 dir = Vector3.back;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;

        rig.velocity = dir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameDefine.TAG.PLAYER)
        {
            //敌人子弹打中玩家，逻辑待添加。
            Player PlayerScripts = other.GetComponent<Player>();
            PlayerScripts.CurLife -= 1;
            print("玩家当前生命值 = " + PlayerScripts.CurLife);

            //敌人子弹自毁逻辑
            Destroy(this.gameObject);
        }
    }
}
