using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    GameObject[] prefabGoStones;//石头预制体组
    //public List<GameObject> Stones; // 石头组

    float minX = -5f;
    float maxX = 5f;

    float minZ = 8f;
    float maxZ = 23f;

    private float stonePositionGap = 2f;

    public int waveCount = 3; // 石头一共有多少波
    public int pWaveCount = 5; // 每一波石头生成石头数量
    public float deltaTime; // 石头生成间隔
    public float deltaWaveTime; // 每一波石头的间隔

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

    void Update()
    {

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

            // 这里是做了一个数据防止生成时贴的太近，但因为本身创建后石头会移动，所以很少会有石头创建时就会撞到石头的逻辑。
            //foreach (GameObject oneStone in Stones)
            //{
            //    Vector3 old_position = oneStone.transform.position;
            //    Vector3 tmp = new_position - old_position;

            //    if (tmp.magnitude < stonePositionGap) // 如果两个位置之间的距离（.magnitude）小于2f
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
                // 这里不需要对石头进行统计、因为不做初始判断，节约空间。
                //Stones.Add(new_Stone);
                break;
            }
        }
    }
}
