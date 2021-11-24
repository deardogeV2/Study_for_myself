using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    GameObject[] prefabGoStones;//ʯͷԤ������
    //public List<GameObject> Stones; // ʯͷ��

    float minX = -5f;
    float maxX = 5f;

    float minZ = 8f;
    float maxZ = 23f;

    private float stonePositionGap = 2f;

    public int waveCount = 3; // ʯͷһ���ж��ٲ�
    public int pWaveCount = 5; // ÿһ��ʯͷ����ʯͷ����
    public float deltaTime; // ʯͷ���ɼ��
    public float deltaWaveTime; // ÿһ��ʯͷ�ļ��

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

            // ����������һ�����ݷ�ֹ����ʱ����̫��������Ϊ��������ʯͷ���ƶ������Ժ��ٻ���ʯͷ����ʱ�ͻ�ײ��ʯͷ���߼���
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
                // ���ﲻ��Ҫ��ʯͷ����ͳ�ơ���Ϊ������ʼ�жϣ���Լ�ռ䡣
                //Stones.Add(new_Stone);
                break;
            }
        }
    }
}
