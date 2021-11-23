using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOne : MonoBehaviour
{
    // ���С��һ��������5�ס�Э��������
    public Transform[] cubes;
    void Start()
    {
        cubes = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cubes[i] = transform.GetChild(i);
        }

        StartCoroutine(move());

    }

    IEnumerator move()
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            while (true)
            {
                cubes[i].Translate(0,0.02f,0);
                yield return null;
                if (cubes[i].transform.position.y >= 5f)
                {
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
