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
    void Start()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        rig.velocity = dir * speed;
    }
}
