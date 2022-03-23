using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : StateBase
{
    EStateName stateName = EStateName.Patrol;

    public StatePatrol(ZombieStateControl controller, EStateName eStateName) : base(controller,eStateName)
    {
        // ����һ���µ�Ŀ��㣬����һ��Ŀ���Ҫ��ͬ��
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
        //������������
        controller.animator.SetFloat("Speed", (float)ZombieSpeed.Walk);

        // ���¼������
        distance = Vector2.Distance(selfTransform.position, targetTransform.position);
    }

    float distance;
    public override void action()
    {
        Debug.Log("��ǰ��Ѳ��״̬");
        // ʵʱ�������
        distance = Vector2.Distance(selfTransform.position, targetTransform.position);
        if (distance > 0.5)
        {
            controller.navMeshAgent.SetDestination(targetTransform.position);
        }

    }

    public override void reasion() // ת����ϵ
    {

        if (controller.distanceToPlayer < 10 && !PlayerStateScript.Instance.isDead)
        {
            controller.stateManager.doTranstion(EStateName.Chase);
            return;
        }
        // ��Ѳ�ߵ�ӽ���2��֮���Ϊ�ȴ�״̬
        if (distance < 0.5)
        {
            controller.stateManager.doTranstion(EStateName.Wait);
            return;
        }
    }

    public override void doAfterEnter()
    {
        base.doAfterEnter();
        //������������
        controller.animator.SetFloat("Speed", (float)ZombieSpeed.Walk);

        // ����һ���µ�Ŀ��㣬����һ��Ŀ���Ҫ��ͬ��
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

        // ���¼������
        distance = Vector2.Distance(selfTransform.position, targetTransform.position);

    }

}
