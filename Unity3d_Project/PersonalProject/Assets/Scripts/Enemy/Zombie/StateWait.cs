using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWait : StateBase
{
    public float WaitTime;
    public float InTime;
    public StateWait(ZombieStateControl controller, EStateName eStateName) : base(controller,eStateName)
    {
        WaitTime = 10f;
        InTime = 0f;
    }
    public override void action()
    {
        Debug.Log("��ǰ�ǵȴ�״̬");
        //������������
        controller.animator.SetFloat("Speed", (float)ZombieSpeed.Idel);
    }

    public override void reasion()
    {
        if (controller.distanceToPlayer < 10 && !PlayerStateScript.Instance.isDead)
        {
            controller.stateManager.doTranstion(EStateName.Chase);
            return;
        }

        if (Time.time - InTime > WaitTime)
        {  // �����ȴ�ʱ��,��ΪѲ��ģʽ
            controller.stateManager.doTranstion(EStateName.Patrol);
            InTime = Time.time;

            return;
        }
    }
    public override void doAfterEnter()
    {
        controller.navMeshAgent.SetDestination(controller.transform.position);

        base.doAfterEnter();

        InTime = Time.time;
    }
}
