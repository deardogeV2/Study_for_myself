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

    // ��������
    public bool isDead;
    //����ֵ
    public int lifePoint;
    // ������
    public int attackPoints;
    // ������Χ
    public float attackDistance = 2f;
    void Start()
    {
        // ��ȡ���е�Ѳ�ߵ�
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

        // ��ȡ��Ƶ����
        AudioPatrol = transform.Find("Audio/patrol").GetComponent<AudioSource>();
        AudioChase = transform.Find("Audio/chase").GetComponent<AudioSource>();
        AudioAttack = transform.Find("Audio/attack").GetComponent<AudioSource>();
        AudioDead = transform.Find("Audio/dead").GetComponent<AudioSource>();

        // ��Ƶ���ſ���
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
        // ���Ѳ��״̬
        StatePatrol patrol = new StatePatrol(this, EStateName.Patrol);
        stateManager.addState(patrol);

        // ��ӵȴ�״̬
        StateWait wait = new StateWait(this, EStateName.Wait);
        stateManager.addState(wait);

        // ���׷��״̬
        StateChase chase = new StateChase(this, EStateName.Chase);
        stateManager.addState(chase);

    }

    void Update()
    {
        // ����һ����ҺͶ������
        distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (!isDead)
        {
            // ���״̬�еķ���
            stateManager.CurState.action();
            stateManager.CurState.reasion();

            // ��ǰ״̬�ж��Լ���Ч�޸�
            if (stateManager.CurState.StateName == EStateName.Patrol)
            {
                curStateAudio = AudioPatrol;

                // ��Ч����
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
            // �ж���Һͽ�ʬ�泯����
            if (directionJudgmentForward())animator.SetBool("FromBack", false);
            else animator.SetBool("FromBack", true);

            animator.SetBool("Dead",true);

            // ����֮����ɾ��С��ͼ�ϵĵ�
            Destroy(transform.Find("MiniMapRed").gameObject);


            // �ر�������Ч
            AudioPatrol.gameObject.SetActive(false);
            AudioChase.gameObject.SetActive(false);
            // ����˲�䲥��������Ч
            AudioDead.Play();
            

            // ����֮��, ����ɾ��������
            Destroy(navMeshAgent);
            Destroy(transform.Find("Z_Body").GetComponent<CapsuleCollider>());
            Destroy(transform.Find("Z_Head").GetComponent<CapsuleCollider>());

            // ����֮�� , 20��ɾ������
            Destroy(gameObject.transform.parent.gameObject,20f);

            // ɾ����ʬ�����еĽ�ʬ
            TaskManager taskManager = GameObject.Find("Map").GetComponent<TaskManager>();
            taskManager.enemys.Remove(transform.parent);

            // ֹͣ�ù��޽ű����������ֹ���������첽ɾ���߼���
            this.enabled = false;
        }


    }

    public void ZombieAttack(string a)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            // ��������ײ
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
