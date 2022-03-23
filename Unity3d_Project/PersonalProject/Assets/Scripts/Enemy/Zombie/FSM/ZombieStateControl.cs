using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStateControl : MonoBehaviour
{
    public Transform[] patrolPoints;
    public StateManager stateManager;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public AnimatorStateInfo animatorStateInfo;
    public Transform Player;
    public float distanceToPlayer;

    [HideInInspector]
    public AudioSource AudioPatrol;
    [HideInInspector]
    public AudioSource AudioChase;
    [HideInInspector]
    public AudioSource AudioAttack;
    [HideInInspector]
    public AudioSource AudioDead;
    [HideInInspector]
    public AudioSource curStateAudio;

    // 生命特征
    public bool isDead;
    //生命值
    public int lifePoint;
    // 攻击力
    public int attackPoints;
    // 攻击范围
    public float attackDistance = 2f;
    void Start()
    {
        // 获取所有的巡逻点
        GameObject allPoint = GameObject.Find("EnemyPatrolPoint");
        int count = allPoint.transform.childCount;
        patrolPoints = new Transform[count];
        int i = 0;
        foreach (Transform item in allPoint.transform)
        {
            patrolPoints[i] = item;
            i++;
        }
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        lifePoint = Random.Range(100, 200);
        isDead = false;
        attackPoints = Random.Range(10, 100);
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 获取音频内容
        AudioPatrol = transform.Find("Audio/patrol").GetComponent<AudioSource>();
        AudioChase = transform.Find("Audio/chase").GetComponent<AudioSource>();
        AudioAttack = transform.Find("Audio/attack").GetComponent<AudioSource>();
        AudioDead = transform.Find("Audio/dead").GetComponent<AudioSource>();

        // 音频播放控制
        AudioPatrol.mute = true;
        AudioPatrol.Play();
        AudioPatrol.loop = true;
        AudioChase.mute = true;
        AudioChase.Play();
        AudioChase.loop = true;

        makeFsm();

    }
    public void makeFsm()
    {
        stateManager = new StateManager();
        // 添加巡逻状态
        StatePatrol patrol = new StatePatrol(this, EStateName.Patrol);
        stateManager.addState(patrol);

        // 添加等待状态
        StateWait wait = new StateWait(this, EStateName.Wait);
        stateManager.addState(wait);

        // 添加追击状态
        StateChase chase = new StateChase(this, EStateName.Chase);
        stateManager.addState(chase);

    }

    void Update()
    {
        // 计算一下玩家和对象距离
        distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (!isDead)
        {
            // 存活状态中的方法
            stateManager.CurState.action();
            stateManager.CurState.reasion();

            // 当前状态判断以及音效修改
            if (stateManager.CurState.StateName == EStateName.Patrol)
            {
                curStateAudio = AudioPatrol;

                // 音效控制
                AudioChase.mute = true;
                AudioPatrol.mute = false;
            }
            else if(stateManager.CurState.StateName == EStateName.Chase)
            {
                AudioPatrol.mute = true;
                AudioChase.mute = false;
            }
            else
            {
                AudioPatrol.mute = true;
                AudioChase.mute = true;
            }
        }
        else
        {
            // 判断玩家和僵尸面朝方向
            if (directionJudgmentForward())animator.SetBool("FromBack", false);
            else animator.SetBool("FromBack", true);

            animator.SetBool("Dead",true);

            // 死亡之后先删除小地图上的点
            Destroy(transform.Find("MiniMapRed").gameObject);


            // 关闭其他音效
            AudioPatrol.gameObject.SetActive(false);
            AudioChase.gameObject.SetActive(false);
            // 死亡瞬间播放死亡音效
            AudioDead.Play();
            

            // 死亡之后, 立即删除导航器
            Destroy(navMeshAgent);
            Destroy(transform.Find("Z_Body").GetComponent<CapsuleCollider>());
            Destroy(transform.Find("Z_Head").GetComponent<CapsuleCollider>());

            // 死亡之后 , 20后删除对象
            Destroy(gameObject.transform.parent.gameObject,20f);

            // 删除僵尸数组中的僵尸
            TaskManager taskManager = GameObject.Find("Map").GetComponent<TaskManager>();
            taskManager.enemys.Remove(transform.parent);

            // 停止该怪兽脚本，此项不会阻止已启动的异步删除逻辑。
            this.enabled = false;
        }


    }

    public void ZombieAttack(string a)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            // 发生了碰撞
            if (hit.collider.transform.tag == "Player")
            {
                PlayerStateScript.Instance.injured(attackPoints);
            }
        }
        
    }

    public bool directionJudgmentForward()
    {
        Vector3 change  = Player.position-transform.position;
        float angleForward = Vector3.Angle(new Vector3(transform.forward.x, 0, transform.forward.z),new Vector3(change.x,0, change.z));
        if (angleForward < 90) return true;
        else return false;
    }

    public void injured(int damage)
    {
        lifePoint-=damage;
        if (lifePoint <= 0)
        {
            isDead = true;
            lifePoint = 0;
        }
    }



}
