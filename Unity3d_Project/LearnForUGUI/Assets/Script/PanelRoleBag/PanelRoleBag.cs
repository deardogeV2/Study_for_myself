using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRoleBag : MonoBehaviour {
    public ItemGrid[] items;//面板上所有的背包格子
	
	void Start () {
        GameObject goBag = GameObject.Find("Bag");
        items = goBag.GetComponentsInChildren<ItemGrid>();

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.G))
        {
            int randId = Random.Range(0, ConfigItem.ItemCount);
            print(randId);
            ConfigItem data = ConfigItem.Get(randId);

            print("点击了G");
            print(storeItemToBag(data));
        }
	}
    /// <summary>
    /// 从已经存储的物品中找ID相同的物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private ItemGrid findTheSameItem(int id)
    {
        foreach (ItemGrid s in items)
        {
            if (s.isStored && s.Data.id == id)
            {
                return s;
            }
        }

        return null;
    }
    /// <summary>
    /// 从所有格子中查找一个空格子
    /// </summary>
    /// <returns></returns>
    private ItemGrid findEmptyItem()
    {
        foreach (ItemGrid s in items)
        {
            if (!s.isStored)//没有存放数据
            {
                return s;
            }
        }

        //没有找到空的物品槽，说明背包以满
        return null;
    }
    /// <summary>
    /// 存储物品到背包中
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public bool storeItemToBag(ConfigItem d)
    {
        if (d == null) return false;
        print("完成了静态文件读取");
        if (d.capacity == 1)//如果该物品在物品槽中只能存放一个
        {
            ItemGrid item = findEmptyItem();//直接找到一个还没有使用的物品格子
            if (item != null)//如果找到了空的物品槽，将该物品放进去格子里
            {
                item.storeItem(d);
            }
            else//如果没有找到，则将说明背包已满
            {
                Debug.Log("没有找到空的物品槽，可能是背包以满");
                return false;
            }
        }
        else if (d.capacity > 1)//物品可以叠加
        {
            //从背包中查找一个相同ID的物品
            ItemGrid item = findTheSameItem(d.id);
            if (item != null)
            {
                item.addItem(1);//如果找到,直接叠加一个单位
            }
            else
            {
                item = findEmptyItem();//如果没有找到已存物体,继续查找空格子
                if (item != null)
                {
                    item.storeItem(d);//找到空格子,直接存放该物品
                }
                else
                {
                    Debug.Log("没有找到空的物品槽，可能是背包以满,需要扩充背包格子!!!");
                    return false;
                }
            }
        }
        return true; ;
    }
}
