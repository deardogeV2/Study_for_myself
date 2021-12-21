using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
//静态数据层
public class ConfigItem
{
    public readonly int id;//物品ID
    public readonly int type;//物品类型
    public readonly string name;//物品名字
    public readonly string descrip;//物品描述
    public readonly string icon;//物品图标
    public readonly int quality;//物品品质
    public readonly int capacity;//物品容量（决定是否可以叠加）
    public ConfigItem() { }
    public ConfigItem(int id, int type, string name, string descrip, string icon, int quality, int capacity)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.descrip = descrip;
        this.icon = icon;
        this.quality = quality;
        this.capacity = capacity;
    }
    
    public static int ItemCount
    {
        get { return dic.Count; }
    }
    //字典容器
    private static Dictionary<int, ConfigItem> dic = new Dictionary<int, ConfigItem>();
    public static ConfigItem Get(int id)
    {
        ConfigItem config;
        if (dic.TryGetValue(id, out config))
        {
            return config;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 用来数据的初始化
    /// </summary>
    public static void init()
    {
        
        loadItem(Application.streamingAssetsPath+@"/item.xml");
    }
    public static void loadItem(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.UTF8);//数据流    
        XmlDocument doc = new XmlDocument();//XML文档
        doc.Load(sr);
        XmlNodeList nodeItems;//节点列表
        nodeItems = doc.SelectNodes("/Items/Item");//使用xpath表达式选择文档中所有的stage子节点
        for (int i = 0; i < nodeItems.Count; i++)
        {
            XmlNode nodeItem = nodeItems[i];
            int id = int.Parse(nodeItem.Attributes["id"].Value);
            if (dic.ContainsKey(id))
            {
                Debug.LogWarning("配置了同样的id:" + id);
                return;
            }
            int type = int.Parse(nodeItem.Attributes["type"].Value);
            string name = nodeItem.Attributes["name"].Value;
            string descrip = nodeItem.Attributes["descrip"].Value;
            string icon = nodeItem.Attributes["icon"].Value;
            int quality = int.Parse(nodeItem.Attributes["quality"].Value);
            int capacity = int.Parse(nodeItem.Attributes["capacity"].Value);
            ConfigItem data = new ConfigItem(id, type, name, descrip, icon, quality, capacity);//生成一个物品对象
            dic.Add(id, data);//把关卡对象放入到字典中
        }
        sr.Close();
        sr.Dispose();
    }
}