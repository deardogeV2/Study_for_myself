using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于存放人物的道具栏的单例
/// </summary>
public class ItemScript
{
    static private ItemScript instance;
    static public ItemScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemScript();
            }
            return instance;
        }
    }

    public int bullet_semi = 20;
    public int bullet_auto = 20;

}
