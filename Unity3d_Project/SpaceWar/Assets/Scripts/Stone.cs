using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Stone : MonoBehaviour
{
    public Vector3 dir = new Vector3(0,0,0);
    public float speed = 3f;
    public float angelSpeed = 3f;
    Rigidbody rig;
    CapsuleCollider capsuleCollider;

    public GameObject prefabBoomEffect; // ��ըЧ��

    //石头分数
    int point;
    public int Point
    {
        get
        {
            return point;
        }
        set
        {
            point = value;
        }
    }

    private void Reset()
    {
        // ע�⣡����ط��Ǳ༭��ʱ���ȡ�Ķ���rig�Լ�capsuleCollider��������ģʽ����ʵ��û�л�ȡ�ģ���ʱStart�����rig�Լ�capsuleCollider��û�б���ֵ�����Իᱬ����ָ��һ�����������»�ȡһ�Ρ�
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;

        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
    }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;
        prefabBoomEffect = Resources.Load<GameObject>("Effect/Explosions/explosion_asteroid");

        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
    }
    void Start()
    {
        Point = Random.Range(10, 50);
        speed = Random.Range(5f, 10f);
        dir = Random.insideUnitSphere;
        rig.angularVelocity = dir * angelSpeed;

        rig.velocity = new Vector3(0, 0, -speed);
    }

    
}
