using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChase : StateBase
{
    public float ChaseDistance = 10f;
    float lastAttackTime;
    public float AttackCd = 3f;
    public StateChase(ZombieStateControl controller, EStateName eStateName) : base(controller, eStateName)
    {
        // 重新给定攻击时间
        lastAttackTime = Time.time;

        // 修改导航系统的方向
        controller.navMeshAgent.SetDestination(controller.Player.position);
        // 修改怪物动画参数
        controller.animator.SetFloat("Speed", 3);
    }

    public override void doAfterEnter()
    {
        base.doAfterEnter();
        // 重新给定攻击时间
        lastAttackTime = Time.time;

        // 修改导航系统的方向
        controller.navMeshAgent.SetDestination(controller.Player.position);
        // 修改怪物动画参数
        controller.animator.SetFloat("Speed", 3);

    }


    public override void action()
    {
        Debug.Log("当前是追击状态");
        // 攻击判定
        if (Time.time - lastAttackTime > AttackCd && controller.distanceToPlayer < 2)
        {
            // 触发攻击
            controller.animatorStateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            if (!controller.animatorStateInfo.IsName("Z_Attack"))
            {
                controller.animator.SetTrigger("Attack");
                lastAttackTime = Time.time;

                // 播放攻击音效
                controller.AudioAttack.Play();
            }
        }

        // 距离判定动作
        if (controller.distanceToPlayer > 3)
        {
            // 修改怪物动画参数
            controller.animator.SetFloat("Speed", 3);
            // 修改导航系统的方向
            controller.navMeshAgent.SetDestination(controller.Player.position);
            // 修改追击速度
            controller.navMeshAgent.speed = 2;
        }
        else if(controller.distanceToPlayer > 2)
        {
            // 修改怪物动画参数
            controller.animator.SetFloat("Speed", 2);
            // 修改导航系统的方向
            controller.navMeshAgent.SetDestination(controller.Player.position);
            // 修改追击速度
            controller.navMeshAgent.speed = 1;
        }
        else
        {
            if (controller.distanceToPlayer < 1.8)
            {
                // 修改怪物动画参数
                controller.animator.SetFloat("Speed", 1);
            }
            // 添加站住时候的旋转逻辑
            float angle = Vector3.Angle(selfTransform.forward, new Vector3((controller.Player.position - selfTransform.position).x, 0, (controller.Player.position - selfTransform.position).z));
            if (angle > 10)
            {
                // 额外添加一点转向逻辑
                selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, Quaternion.LookRotation(controller.Player.position - selfTransform.position), Time.deltaTime * 10f);
            }
        }
    }

    public override void reasion()
    {
        if(controller.distanceToPlayer > ChaseDistance || PlayerStateScript.Instance.isDead) // 超出设定长度 或者主角死亡自动变为等待
        {
            controller.stateManager.doTranstion(EStateName.Wait);
        }
    }

    

}
