using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ڴ������ĵ������ĵ���
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
