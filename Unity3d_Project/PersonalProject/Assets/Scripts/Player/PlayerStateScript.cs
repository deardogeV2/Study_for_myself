using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateScript : MonoBehaviour
{
    public float lifePoint;
    public float fullLifePoint = 1000f;
    public bool isDead = false;
    private static PlayerStateScript instance;
    private Image LifePointUI;
    public static PlayerStateScript Instance
    {
        get { return instance; }
    }

    void Start()
    {
        lifePoint = Random.Range(100, 1000);
        instance = GetComponent<PlayerStateScript>();
        LifePointUI = GameObject.Find("GameUI/Canvas/Game/DownRight/LeftPoint/NowLifePoint").GetComponent<Image>();

        if (instance == null) {
            GameObject go = new GameObject();
            go.AddComponent<PlayerStateScript>();
            instance = go.GetComponent<PlayerStateScript>();
        }
    }
    Transform UIlifePoint;
    // Update is called once per frame
    void Update()
    {
        if (lifePoint > 0)
        {
            print("人物还活着");
        }
        else
        {
            print("人物已经死亡");
            lifePoint = 0;
            isDead = true;
        }
        LifePointUI.fillAmount = lifePoint/fullLifePoint;
    }

    public void injured(int damage)
    {
        lifePoint-=damage;
    }
}
