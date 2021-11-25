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

    public GameObject prefabBoomEffect; // 爆炸效果


    private void Reset()
    {
        // 注意！这个地方是编辑器时候获取的对象rig以及capsuleCollider，在运行模式下其实是没有获取的，此时Start里面的rig以及capsuleCollider还没有被赋值，所以会爆出空指针一场，必须重新获取一次。
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
        if (other.tag!="GameZoom" && other.tag != "Stone" && other.tag != "Enemy")
        {
            Destroy(this.gameObject);// 销毁被撞到的石头
            Destroy(other.gameObject);// 销毁撞击的子弹 

            // 添加爆炸效果
            if (prefabBoomEffect)
            {
                //创建一个粒子特效对象
                print("对象加载成功");
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
