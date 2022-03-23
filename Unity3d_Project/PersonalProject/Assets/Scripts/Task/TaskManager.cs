using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    Transform[] enemyBornPoints;
    Transform[] enemyPatrolPoints;
    Transform[] itemPoints;
    [HideInInspector]
    public List<Transform> enemys;
    [HideInInspector]
    public List<Transform> items;

    GameObject Enemys;
    GameObject Items;

    GameObject PassUI;
    GameObject ClearUI;

    [Tooltip("控制游戏是否开始")]
    public bool isStart = false;
    public bool shouldStart = false;
    [Tooltip("控制当前关卡数")]
    public int taskNumber = 0;

    public int changeTaskNumber = 1;
    public bool taskIsOver = true;
    public int MaxEnemy = 10;

    bool isPassUIShow = false;
    bool isClearUIShow = false;
    public int ClearTaskNumber = 0;


    int enemyRemainingNumber = 0;

    void Start()
    {
        // 获取三组生成点坐标
        GameObject EnemyBornPoint = GameObject.Find("EnemyBornPoint");
        if (EnemyBornPoint != null)
        {
            enemyBornPoints = new Transform[EnemyBornPoint.transform.childCount];
            int i = 0;
            foreach (Transform item in EnemyBornPoint.transform)
            {
                enemyBornPoints[i] = item;
                i++;
            }
        }

        GameObject EnemyPatrolPoint = GameObject.Find("EnemyPatrolPoint");
        if (EnemyPatrolPoint != null)
        {
            enemyPatrolPoints = new Transform[EnemyPatrolPoint.transform.childCount];
            int i = 0;
            foreach (Transform item in EnemyPatrolPoint.transform)
            {
                enemyPatrolPoints[i] = item;
                i++;
            }
        }

        GameObject ItemPoints = GameObject.Find("ItemPoints");
        if (ItemPoints != null)
        {
            itemPoints = new Transform[ItemPoints.transform.childCount];
            int i = 0;
            foreach (Transform item in ItemPoints.transform)
            {
                itemPoints[i] = item;
                i++;
            }
        }
        // 获取过关和通关UI
        PassUI = GameObject.Find("Canvas").transform.Find("PassUI").gameObject;
        ClearUI = GameObject.Find("Canvas").transform.Find("ClearUI").gameObject;


        // 控制能生成的总道具数
        items = new List<Transform>();
        enemys = new List<Transform>();

        // 创建父物体来管理道具
        Items = new GameObject();
        Items.name = "Items";

        // 创建父物体来管理敌人
        Enemys = new GameObject();
        Enemys.name = "Enemys";
    }

    private void Update()
    {
        // 道具开始生成判断
        if (!isStart && shouldStart == true)
        {
            shouldStart = isStart = true;
            StartCoroutine(creatItem());
        }


        // 剩余敌人数量更新
        updateEnemyNumber();

        // 过关UI判断
        passJudge();

        // 关卡切换判断
        if (isStart && changeTaskNumber != taskNumber)
        {
            // 检查协程、开启新的协程
            if (taskIsOver == true)
            {
                // 开启新的协程
                StartCoroutine(taskMaker(changeTaskNumber));
            }
            // 修改预期关卡
            taskNumber = changeTaskNumber;
        }

        if (taskIsOver && enemyRemainingNumber==0 & changeTaskNumber<5)
        {
            changeTaskNumber += 1;
        }
    }

    void updateEnemyNumber()
    {
        enemyRemainingNumber = Enemys.transform.childCount;
    }

    void passJudge()
    {
        if (isPassUIShow || isClearUIShow) return;

        if (taskNumber >= 1 && taskIsOver && enemyRemainingNumber == 0)
        {
            if (taskNumber == 5) StartCoroutine(showClearUI());
            else if (ClearTaskNumber != taskNumber)
            {
                ClearTaskNumber = taskNumber;
                StartCoroutine(showPassUI(taskNumber));
            };
        }


    }

    IEnumerator showPassUI(int wave = 1, int t = 20)
    {

        isPassUIShow = true;
        PassUI.SetActive(true);
        Text text = PassUI.GetComponent<Text>();
        text.text = $"第{wave}波结束\n休息时间20秒";
        yield return new WaitForSeconds(1);
        PassUI.SetActive(false);
        yield return new WaitForSeconds(1);
        PassUI.SetActive(true);
        yield return new WaitForSeconds(1);
        PassUI.SetActive(false);
        yield return new WaitForSeconds(1);
        PassUI.SetActive(true);
        yield return new WaitForSeconds(3);
        PassUI.SetActive(false);
        isPassUIShow = false;
    }

    IEnumerator showClearUI()
    {
        isClearUIShow = true;
        ClearUI.SetActive(true);
        yield return new WaitForSeconds(10);
        isClearUIShow = false;
    }


    IEnumerator creatItem()
    {
        while (true)
        {
            if (items.Count < itemPoints.Length)
            {
                while (true)
                {
                    bool isOk = false;
                    int num = Random.Range(0, itemPoints.Length);
                    Vector3 newOne = itemPoints[num].position;
                    foreach (Transform item in items)
                    {
                        float distance = Vector3.Distance(item.position, newOne);
                        if (distance > 3f)
                        {
                            isOk = true;
                            break;
                        }
                    }
                    if (isOk || items.Count == 0)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Prefabs/items/" + (Random.Range(0, 2) == 0 ? "bulletAuto" : "bulletSemi"));
                        GameObject go = Instantiate(prefab);
                        go.transform.parent = Items.transform;
                        go.transform.position = newOne;
                        items.Add(go.transform);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    // 关卡生成内容暂时写在一个协程里面
    IEnumerator taskMaker(int taskNum)
    {
        // 关卡开始
        taskIsOver = false;
        yield return new WaitForSeconds(20);
        int Wave = 0;
        int ZombieNum = 0;
        int BossNum = 0;

        switch (taskNum)
        {
            case 1:
                // 第一关，数值设定
                Wave = 1;
                ZombieNum = 1;
                break;
            case 2:
                Wave = 1;
                ZombieNum = 1;
                break;
            case 3:
                Wave = 1;
                ZombieNum = 1;
                break;
            case 4:
                Wave = 1;
                ZombieNum = 2;
                break;
            case 5:
                Wave = 1;
                ZombieNum = 3;
                break;
            default:
                break;
        }
        // 生成指定数量的敌人
        while (Wave > 0)
        {
            int t = 0;
            while (t < ZombieNum)
            {
                creatOneEnemy("Zombie");
                t++;
                yield return new WaitForSeconds(2);
            }
            Wave--;
            yield return new WaitForSeconds(10);
        }

        // 关卡结束
        taskIsOver = true;
        yield return new WaitForSeconds(5);
    }


    void creatOneEnemy(string name = "Zombie")
    {
        if (enemys.Count < MaxEnemy)
        {
            while (true)
            {
                Vector3 newOne = enemyBornPoints[Random.Range(0, enemyBornPoints.Length)].position;
                bool isOk = false;
                foreach (Transform item in enemys)
                {
                    float distance = Vector3.Distance(item.position, newOne);
                    if (distance > 2f)
                    {
                        isOk = true;
                        break;
                    }
                }

                if (isOk || enemys.Count == 0)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Monster/" + name);
                    if (prefab != null)
                    {
                        GameObject go = Instantiate(prefab);
                        go.transform.parent = Enemys.transform;
                        go.transform.position = newOne;
                        enemys.Add(go.transform);
                    }
                    break;
                }
            }
        }
    }

}
