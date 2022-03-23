using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStateName
{
    None,
    Wait, // µÈ´ý×´Ì¬
    Attack, // ¹¥»÷
    Follow, // ¸úËæµÐÈË
    Patrol, // Ñ²Âß
    Chase,

}

public enum ETransition
{
    None,
    Patrol_To_Follow,
    Patrol_To_Attack,
    Follow_To_Patrol,
    Follow_To_Attack,
    Attack_To_Follow,
    Attack_To_Patrol,
}

public enum ZombieSpeed
{
    Idel = 0,
    Walk = 2,
    Run = 3
}

public abstract class StateBase
{
    protected ZombieStateControl controller;

    protected EStateName stateName;
    public EStateName StateName
    {
        get { return stateName; }
    }
    protected Transform selfTransform;
    protected Transform targetTransform;
    protected int patrolPointNumber = 0;

    public StateBase(ZombieStateControl controller, EStateName eStateName)
    {
        this.controller = controller;
        selfTransform = this.controller.transform;
        this.stateName = eStateName;
    }

    public virtual void doBeforLeaving()
    {

    }
    public virtual void doAfterEnter()
    {

    }

    public abstract void action();
    public abstract void reasion();
}
