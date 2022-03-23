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
        // ���¸�������ʱ��
        lastAttackTime = Time.time;

        // �޸ĵ���ϵͳ�ķ���
        controller.navMeshAgent.SetDestination(controller.Player.position);
        // �޸Ĺ��ﶯ������
        controller.animator.SetFloat("Speed", 3);
    }

    public override void doAfterEnter()
    {
        base.doAfterEnter();
        // ���¸�������ʱ��
        lastAttackTime = Time.time;

        // �޸ĵ���ϵͳ�ķ���
        controller.navMeshAgent.SetDestination(controller.Player.position);
        // �޸Ĺ��ﶯ������
        controller.animator.SetFloat("Speed", 3);

    }


    public override void action()
    {
        Debug.Log("��ǰ��׷��״̬");
        // �����ж�
        if (Time.time - lastAttackTime > AttackCd && controller.distanceToPlayer < 2)
        {
            // ��������
            controller.animatorStateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            if (!controller.animatorStateInfo.IsName("Z_Attack"))
            {
                controller.animator.SetTrigger("Attack");
                lastAttackTime = Time.time;

                // ���Ź�����Ч
                controller.AudioAttack.Play();
            }
        }

        // �����ж�����
        if (controller.distanceToPlayer > 3)
        {
            // �޸Ĺ��ﶯ������
            controller.animator.SetFloat("Speed", 3);
            // �޸ĵ���ϵͳ�ķ���
            controller.navMeshAgent.SetDestination(controller.Player.position);
            // �޸�׷���ٶ�
            controller.navMeshAgent.speed = 2;
        }
        else if(controller.distanceToPlayer > 2)
        {
            // �޸Ĺ��ﶯ������
            controller.animator.SetFloat("Speed", 2);
            // �޸ĵ���ϵͳ�ķ���
            controller.navMeshAgent.SetDestination(controller.Player.position);
            // �޸�׷���ٶ�
            controller.navMeshAgent.speed = 1;
        }
        else
        {
            if (controller.distanceToPlayer < 1.8)
            {
                // �޸Ĺ��ﶯ������
                controller.animator.SetFloat("Speed", 1);
            }
            // ���վסʱ�����ת�߼�
            float angle = Vector3.Angle(selfTransform.forward, new Vector3((controller.Player.position - selfTransform.position).x, 0, (controller.Player.position - selfTransform.position).z));
            if (angle > 10)
            {
                // �������һ��ת���߼�
                selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, Quaternion.LookRotation(controller.Player.position - selfTransform.position), Time.deltaTime * 10f);
            }
        }
    }

    public override void reasion()
    {
        if(controller.distanceToPlayer > ChaseDistance || PlayerStateScript.Instance.isDead) // �����趨���� �������������Զ���Ϊ�ȴ�
        {
            controller.stateManager.doTranstion(EStateName.Wait);
        }
    }

    

}
