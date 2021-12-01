using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    GameObject[] prefabGoStones;//石头预制体组，用作随机生成不同模型的石头
    //public List<GameObject> Stones; // ʯͷ��

    float minX = -5f;
    float maxX = 5f;

    float minZ = 8f;
    float maxZ = 23f;

    private float stonePositionGap = 2f;

    public int waveCount = 3; // 总石头波数
    public int pWaveCount = 5; // 每一波石头的数量
    public float deltaTime; // 石头生成的间隔时间
    public float deltaWaveTime; // 每一波间隔时间

    private void Awake()
    {
        deltaTime = Random.Range(0.5f, 1.5f);
        deltaWaveTime = Random.Range(2.0f, 5.0f);

        prefabGoStones = new GameObject[3];
        for (int i = 0; i < prefabGoStones.Length; i++)
        {
            prefabGoStones[i] = Resources.Load<GameObject>("Stone" + i);
        }
    }
    void Start()
    {
        StartCoroutine(CreatStones());
    }

    IEnumerator CreatStones()
    {
        yield return new WaitForSeconds(deltaWaveTime);
        for (int i = 0; i < waveCount; i++)
        {
            for (int j = 0; j < pWaveCount; j++)
            {
                CreatOneStone();
                yield return new WaitForSeconds(deltaTime);
            }
            yield return new WaitForSeconds(deltaWaveTime);
        }
    }
    private void CreatOneStone()
    {
        while (true)
        {
            GameObject theStonePrefab = prefabGoStones[(int)Random.Range(1f, 3f)];

            float x = Random.Range(minX, maxX);
            float y = 2f;
            float z = Random.Range(minZ, maxZ);

            Vector3 new_position = new Vector3(x, y, z);

            bool tmp_result = true;

            // 这里是检查新随机点与已有石头之间的距离大于要求值，意图生成新石头时不出现重叠，但是因为本次石头开始就有初速度进行移动所以没必要做此判断
            //foreach (GameObject oneStone in Stones)
            //{
            //    Vector3 old_position = oneStone.transform.position;
            //    Vector3 tmp = new_position - old_position;

            //    if (tmp.magnitude < stonePositionGap) // �������λ��֮��ľ��루.magnitude��С��2f
            //    {
            //        tmp_result = false;
            //        break;
            //    }
            //}

            if (tmp_result)
            {
                GameObject new_Stone = Instantiate<GameObject>(theStonePrefab);
                new_Stone.transform.SetParent(this.gameObject.transform);
                new_Stone.transform.position = new_position;
                new_Stone.GetComponent<Stone>().speed = Random.Range(1f, 5f);
                // 因为不做石头整体管理，所以不需要将游戏对象加入数组进行管理
                //Stones.Add(new_Stone);
                break;
            }
        }
    }
}
