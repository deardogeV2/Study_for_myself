using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFilePanel : MonoBehaviour
{
    //游戏界面对象
    Text textLife;
    Text textPoint;
    void Start()
    {
        // 理论上来说分层查找会安全，但实际上这里可以直接将生命值和分数值命名为唯一的，如ValueCurLife然后直接Find
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
