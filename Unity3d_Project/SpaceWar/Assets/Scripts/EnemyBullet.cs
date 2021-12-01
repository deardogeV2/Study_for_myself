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
            //�����ӵ�������ң��߼�����ӡ�
            Player PlayerScripts = other.GetComponent<Player>();
            PlayerScripts.CurLife -= 1;
            print("��ҵ�ǰ����ֵ = " + PlayerScripts.CurLife);

            //�����ӵ��Ի��߼�
            Destroy(this.gameObject);
        }
    }
}
