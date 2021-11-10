using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodControl : MonoBehaviour
{
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision) // 碰撞触发触发函数。控件选择了 is Tragger后无法触发
    {
        DestroyObject(this.gameObject);//删除游戏组件, this 是指这个脚本控件。而this.gameObject可以获取控件所在游戏组件的对象。
    }

    private void OnTriggerEnter(Collider other) // 触发器触发函数。
    {
        DestroyObject(this.gameObject);//删除游戏组件, this 是指这个脚本控件。而this.gameObject可以获取控件所在游戏组件的对象。
    }
}
