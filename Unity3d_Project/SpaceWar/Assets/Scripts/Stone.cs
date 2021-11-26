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
        
        speed = Random.Range(5f, 10f);
        dir = Random.insideUnitSphere;
        rig.angularVelocity = dir * angelSpeed;

        rig.velocity = new Vector3(0, 0, -speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag!="GameZoom" && other.tag != "Stone" && other.tag != "Enemy" && other.tag != "EnemyBullet")
        {
            Destroy(this.gameObject);// ���ٱ�ײ����ʯͷ
            Destroy(other.gameObject);// ����ײ�����ӵ� 

            // ���ӱ�ըЧ��
            if (prefabBoomEffect)
            {
                //����һ��������Ч����
                print("������سɹ�");
                GameObject boom = Instantiate(prefabBoomEffect, transform.position, Quaternion.identity);

                Destroy(boom, 1.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
