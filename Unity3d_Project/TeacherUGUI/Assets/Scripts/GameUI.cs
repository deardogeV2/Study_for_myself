using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : SingletonMono<GameUI> {

    private RectTransform rect;

    private PanelRoleBag bag;
    public bool isPickItem;//鼠标上是否拾取了物品

    private MouseItem mouseItem;//鼠标上的物品
    private Vector3 itemMouseOldPos;//保存鼠标上物品的初始位置
    public MouseItem MouseItem
    {
        get { return mouseItem; }
        set { mouseItem = value; }
    }


    //物品提示框
    public bool isShowItemInfo;//是否显示物品信息提示框
    private PanelMsg itemInfo;//物品的信息提示

    /// <summary>
    /// 隐藏鼠标拾取物品
    /// </summary>
    public void hideMousePickItem()
    {
        mouseItem.transform.localPosition = itemMouseOldPos;
        isPickItem = false;
    }
    public override void Awake()
    {
        base.Awake();
        //配置数据加载
        //print("配置数据调用前");
        ConfigItem.init();
        //print("配置数据调用后");

        bag = transform.Find("PanelRoleBag").GetComponent<PanelRoleBag>();
        mouseItem = transform.Find("MouseItem").GetComponent<MouseItem>();
        itemMouseOldPos = mouseItem.transform.localPosition;
        rect = this.GetComponent<RectTransform>();
        itemInfo = transform.Find("PanelMsg").GetComponent<PanelMsg>();
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //更新物品提示框的位置
        if (isShowItemInfo)
        {
            Vector2 point = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out point);
            itemInfo.transform.localPosition = point;
        }

        //鼠标拾取到物品之后
        if (isPickItem)
        {
            Vector2 point = Vector2.zero;
            //该函数是将屏幕坐标转化以第一个参数对象的子节点坐标
            //参数一：需要转换的坐标以该对象作为父节点
            //参数二：鼠标点
            //参数三：参数一对象以哪个摄像机渲染（由于该参数一画布没有相机渲染，故为null）
            //参数四：返回一个需要转换的目标点
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out point);
            mouseItem.transform.localPosition = point;
        }

        //按空格键盘,模拟生成物品
        if (Input.GetKeyUp(KeyCode.G))
        {
            int randId = Random.Range(0, ConfigItem.ItemCount);//物品ID
            ConfigItem data = ConfigItem.Get(randId);//通过物品ID得到物品的数据信息

            bag.storeItemToBag(data);//通过数据添加物品
        }
	}

    /// <summary>
    /// 显示物品提示信息
    /// </summary>
    /// <param name="str"></param>
    public void showItemInfo(string str = "")
    {
        isShowItemInfo = true;
        itemInfo.show(str);
    }
    /// <summary>
    /// 隐藏物品提示信息
    /// </summary>
    public void hideItemInfo()
    {
        isShowItemInfo = false;
        itemInfo.hide();
    }
}
