using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFilePanel : MonoBehaviour
{
    //��Ϸ�������
    Text textLife;
    Text textPoint;
    void Start()
    {
        // ��������˵�ֲ���һᰲȫ����ʵ�����������ֱ�ӽ�����ֵ�ͷ���ֵ����ΪΨһ�ģ���ValueCurLifeȻ��ֱ��Find
        GameObject ValueLife = GameObject.Find("ValueLife");
        GameObject ValuePoint = GameObject.Find("ValuePoint");

        if (ValueLife)
        {
            textLife = ValueLife.transform.Find("Value").GetComponent<Text>();
        }

        if (ValuePoint)
        {
            textPoint = ValuePoint.transform.Find("Value").GetComponent<Text>();
        }
    }

    public void updateLife(int curLife)
    {
        textLife.text = curLife.ToString();
    }

    public void updatePoint(int curPoint)
    {
        textPoint.text = curPoint.ToString();
    }
}
