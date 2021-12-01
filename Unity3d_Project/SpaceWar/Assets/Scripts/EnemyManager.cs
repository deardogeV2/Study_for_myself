using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameObject prefabGoEnemy; //敌人飞船预制体

    float minX = -5f;
    float maxX = 5f;

    float minZ = 8f;
    float maxZ = 23f;

    public int waveCountEnemy = 3; // 敌人一共有多少波
    public int pWaveCountEnemy = 5; // 每一波敌人生成石头数量
    public float deltaTimeEnemy; // 敌人生成间隔
    public float deltaWaveTimeEnemy; // 每一波敌人的间隔
    void Start()
    {
        deltaTimeEnemy = Random.Range(0.5f, 1.5f);
        deltaWaveTimeEnemy = Random.Range(2.0f, 5.0f);
        prefabGoEnemy = Resources.Load<GameObject>("EnemyShip");

        StartCoroutine(CreatEnemyShip());
    }

    IEnumerator CreatEnemyShip()
    {
        yield return new WaitForSeconds(deltaWaveTimeEnemy);
        for (int i = 0; i < waveCountEnemy; i++)
        {
            for (int j = 0; j < pWaveCountEnemy; j++)
            {
                CreatOneEnemy();
                yield return new WaitForSeconds(deltaTimeEnemy);
            }
            yield return new WaitForSeconds(deltaWaveTimeEnemy);
        }
    }

    private void CreatOneEnemy()
    {
        GameObject theShipPrefab = prefabGoEnemy;

        float x = Random.Range(minX, maxX);
        float y = 2f;
        float z = Random.Range(minZ, maxZ);

        Vector3 new_position = new Vector3(x, y, z);

        GameObject new_Ship = Instantiate<GameObject>(theShipPrefab);
        new_Ship.transform.SetParent(this.gameObject.transform);
        new_Ship.transform.position = new_position;
        new_Ship.GetComponent<EnemyShip>().speed = Random.Range(1f, 5f);
    }
}
