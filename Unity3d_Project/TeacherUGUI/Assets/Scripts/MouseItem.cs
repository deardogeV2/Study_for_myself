using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 鼠标指针上的物品
/// </summary>
public class MouseItem : MonoBehaviour {

    private Image icon;
    private Text num;
    private ItemGrid itemFirstPick;//对第一次鼠标拾取物品的引用

    public ItemGrid Item
    {
        get { return itemFirstPick; }
        set { 
            itemFirstPick = value;
            GameUI.Instance.isPickItem = true;
            num.gameObject.SetActive(false);
            if(itemFirstPick.Data.capacity>1)
            {
                num.text = itemFirstPick.Count.ToString();
                num.gameObject.SetActive(true);
            }
            string iconPath = "file://" + Application.dataPath + "\\ItemIcon\\" + itemFirstPick.Data.icon;
            StartCoroutine(Tools.LoadImage(iconPath, icon));
        }
    }
	void Awake () {
        icon = transform.GetComponent<Image>();
        num = transform.Find("Num").GetComponent<Text>();
	}
}
