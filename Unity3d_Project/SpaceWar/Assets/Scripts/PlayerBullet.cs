using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBullet : MonoBehaviour
{
    // 注意，子弹本身为quad地面，然后附上了一个删除背景色的贴图，并且添加一个Capsule collider（胶囊形态碰撞体）。
    // 所以，实际上子弹素材本身是没有模型厚度的，只有碰撞体体积。
    // 注意，子弹的is Trigger 需要勾上，不然会因为飞船的移动导致子弹受到其它方向的惯性从而发生偏移旋转。
    public Vector3 dir = Vector3.forward;// .up 为向y轴， .forward 为指向Z轴
    public float speed = 5f;

    //爆炸参数
    GameObject prefabStoneEffect;
    GameObject prefabEnemyEffect;

    private void Awake()
    {
        prefabStoneEffect = Resources.Load<GameObject>("Effect/Explosions/explosion_asteroid");
        prefabEnemyEffect = Resources.Load<GameObject>("Effect/Explosions/explosion_enemy");
    }
    void Start()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        rig.velocity = dir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == GameDefine.TAG.ENEMY ||
            other.tag == GameDefine.TAG.STONE ||
            other.tag == GameDefine.TAG.ENEMY_BULLET
            )
        {
            //子弹打中了敌人的飞机/陨石/敌人的子弹
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            if (other.tag == GameDefine.TAG.STONE)
            {
                //陨石特有逻辑
                Player.Instance.CurPoint += other.transform.GetComponent<Stone>().Point;

                //爆炸逻辑、注意，需要先判断爆炸特效是否加载成功。
                if (prefabStoneEffect)
                {
                    GameObject boom = Instantiate(prefabStoneEffect, transform.position, Quaternion.identity);
                    Destroy(boom, 1.5f);
                }
            }
            else if (other.tag == GameDefine.TAG.ENEMY)
            {
                //敌人飞机特有逻辑
                //加分
                Player.Instance.CurPoint += other.transform.GetComponent<EnemyShip>().Point;

                //爆炸逻辑、注意，需要先判断爆炸特效是否加载成功。
                if (prefabStoneEffect)
                {
                    GameObject boom = Instantiate(prefabEnemyEffect, transform.position, Quaternion.identity);
                    Destroy(boom, 1.5f);
                }
            }
        }
    }
}
