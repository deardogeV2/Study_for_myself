using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkTwo_Sunkens : MonoBehaviour
{
    // �ڶ��⣬��һ���ش̣�һ��ʱ���ؼ���ʧ��һ��ʱ������
    void Start()
    {
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        while (true)
        {
            while (true)
            {
                transform.Translate(0, 0.02f, 0);
                yield return null;
                if (transform.position.y >= 1f)
                {
                    break;
                }
            }

            yield return new WaitForSeconds(1.5f);

            while (true)
            {
                transform.Translate(0, -0.01f, 0);
                yield return null;
                if (transform.position.y <= -1f)
                {
                    break;
                }
            }

            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
