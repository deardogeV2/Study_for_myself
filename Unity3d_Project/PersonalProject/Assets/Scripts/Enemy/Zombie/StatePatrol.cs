using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : StateBase
{
    EStateName stateName = EStateName.Patrol;

    public StatePatrol(ZombieStateControl controller, EStateName eStateName) : base(controller,eStateName)
    {
        // 计算一个新的目标点，与上一个目标点要求不同。
        while (true)
        {
            int i = Random.Range(0, controller.patrolPoints.Length - 1);
            if (i != patrolPointNumber)
            {
                targetTransform = controller.patrolPoints[i];
                patrolPointNumber = i;
                break;
            }
        }
        //动画参数控制
        controller.animator.SetFloat("Speed", (float)ZombieSpeed.Walk);

        // 重新计算距离
        distance = Vector2.Distance(selfTransform.position, targetTransform.position);
    }

    float distance;
    public override void action()
    {
        Debug.Log("当前是巡逻状态");
        // 实时计算距离
        distance = Vector2.Distance(selfTransform.position, targetTransform.position);
        if (distance > 0.5)
        {
            controller.navMeshAgent.SetDestination(targetTransform.position);
        }

    }

    public override void reasion() // 转换关系
    {

        if (controller.distanceToPlayer < 10 && !PlayerStateScript.Instance.isDead)
        {
            controller.stateManager.doTranstion(EStateName.Chase);
            return;
        }
        // 与巡逻点接近至2米之后变为等待状态
        if (distance < 0.5)
        {
            controller.stateManager.doTranstion(EStateName.Wait);
            return;
        }
    }

    public override void doAfterEnter()
    {
        base.doAfterEnter();
        //动画参数控制
        controller.animator.SetFloat("Speed", (float)ZombieSpeed.Walk);

        // 计算一个新的目标点，与上一个目标点要求不同。
        while (true)
        {
            int i = Random.Range(0, controller.patrolPoints.Length - 1);
            if (i != patrolPointNumber)
            {
                targetTransform = controller.patrolPoints[i];
                patrolPointNumber = i;
                break;
            }
        }

        // 重新计算距离
        distance = Vector2.Distance(selfTransform.position, targetTransform.position);

    }

}
