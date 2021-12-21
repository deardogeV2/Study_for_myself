using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemGrid : EventTrigger
{
    private Image icon;
    private Image lockFlag;
    private Text textNum;
    //--------------物品数据
    ConfigItem data;
    private int count;//物品数量

    public int Count
    {
        get { return count; }
    }

    public ConfigItem Data
    {
        get { return data; }
        set
        {
            data = value;
            if(data==null)
            {
                lockFlag.gameObject.SetActive(false);
                textNum.gameObject.SetActive(false);
                icon.gameObject.SetActive(false);
            }
            else
            {
                //通过配置数据去资源目录下读取图片
                //icon.sprite = Resources.Load<Sprite>(data.icon);
                string iconPath = "file://" + Application.dataPath + "\\ItemIcon\\" + data.icon;//Application.dataPath:指的是Assets文件夹所在的目录
                StartCoroutine(Tools.LoadImage(iconPath, icon));
            }
            
        }
    }
    //判断是否已经存储了物品
    public bool isStored
    {
        get
        {
            return icon.gameObject.activeInHierarchy;
        }
    }
    //判断格子是否被锁住
    public bool isLocked
    {
        get
        {
            return lockFlag.gameObject.activeInHierarchy;
        }
    }
    void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        lockFlag = transform.Find("Lock").GetComponent<Image>();
        textNum = transform.Find("Num").GetComponent<Text>();

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //鼠标进入格子，显示格子上物品的信息
        if (isStored && !GameUI.Instance.isPickItem)
        {
            if(data!=null)
            {
                GameUI.Instance.showItemInfo(data.descrip);
            }
            
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        print("OnPointerDown");
        if (!isStored)//格子里还没有存物品
        {
            if (GameUI.Instance.isPickItem)//鼠标上已经拾取
            {
                //icon.gameObject.SetActive(true);
                storeItem(GameUI.Instance.MouseItem.Item.data, GameUI.Instance.MouseItem.Item.count);
                GameUI.Instance.hideMousePickItem();//隐藏鼠标上的物品
            }
        }
        else//格子里有物品
        {
            if (GameUI.Instance.isPickItem)//鼠标上已经拾取
            {
                exchangeItem(GameUI.Instance.MouseItem.Item);
                GameUI.Instance.hideMousePickItem();

            }
            else//鼠标上没有拾取物品
            {
                GameUI.Instance.MouseItem.Item = this;
                icon.gameObject.SetActive(false);
                textNum.gameObject.SetActive(false);
                GameUI.Instance.hideItemInfo();
            }
        }
        //GameUI.Instance.hideItemInfo();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //鼠标离开格子
        if (isStored)
        {
            GameUI.Instance.hideItemInfo();
        }
    }

    /// <summary>
    /// 往已经存放了物品的格子中追加物品
    /// </summary>
    /// <param name="num"></param>
    public void addItem(int num = 1)
    {
        this.count += num;
        if (textNum)
        {
            if (data.capacity > 1)
            {
                textNum.text = this.count.ToString();
                textNum.gameObject.SetActive(true);
            }
            else
            {
                textNum.text = "";
                //textNum.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 存储物品
    /// </summary>
    /// <param name="data"></param>
    /// <param name="num"></param>
    public void storeItem(ConfigItem data, int num = 1)
    {
        textNum.gameObject.SetActive(false);
        //icon.gameObject.SetActive(true);
        Data = data;
        this.count = num;
        if (count > 1)
        {
            textNum.text = count.ToString();
            textNum.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 交换两个格子中的物品
    /// </summary>
    /// <param name="other"></param>
    public void exchangeItem(ItemGrid other)
    {
        ConfigItem otherData = other.data;
        int otherCount = other.count;

        other.storeItem(data, count);

        this.storeItem(otherData, otherCount);
    }


}
