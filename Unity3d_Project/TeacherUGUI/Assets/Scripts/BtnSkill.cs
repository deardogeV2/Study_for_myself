using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSkill : MonoBehaviour 
{
    public float cdTime=1.0f;//技能CD时间
    private float curCdTime;//控制CD时间变化值
    public KeyCode hotKey;//技能的快捷键
    Button btn;
    Text txtCdTime;//显示CD时间
    Image imgMask;//CD蒙板图片
    Text txtHotKey;//显示快捷键

    private bool isCd;//是否在CD中

    //控制isCd变量的属性
    public bool IsCd
    {
        get
        {
            return isCd;
        }

        set
        {
            isCd = value;
            //控制UI对象的显示和隐藏
            txtCdTime.gameObject.SetActive(isCd);
            imgMask.gameObject.SetActive(isCd);
        }
    }
    //控制curCdTime变量的属性
    public float CurCdTime
    {
        get
        {
            return curCdTime;
        }

        set
        {
            curCdTime = value;
            //控制UI内容的显示
            float percent = curCdTime / cdTime;
            imgMask.fillAmount = percent;
            //txtCdTime.text = curCdTime.ToString();
            txtCdTime.text = Math.Round(curCdTime, 2).ToString();
        }
    }

    void Start ()
	{
        btn = GetComponent<Button>();
        txtCdTime = transform.Find("TxtCdTime").GetComponent<Text>();
        imgMask = transform.Find("Mask").GetComponent<Image>();
        txtHotKey = transform.Find("HotKey").GetComponent<Text>();
        //txtHotKey.text = Enum.GetName(typeof(KeyCode), hotKey);
        txtHotKey.text = ((char)hotKey+"").ToUpper();//把按键码对应的枚举值转换成对应的asc码再转换成字符串，再转换成字符串大写
        CurCdTime = cdTime;
        IsCd = false;
    }
	
	void Update () 
	{
		//按键
        if(IsCd==false)
        {
            if(Input.GetKeyDown(hotKey))
            {
                IsCd = true;
            }
        }
        //如果正在CD中，走cd
        if(IsCd==true)
        {
            CurCdTime -= Time.deltaTime;
            if(CurCdTime<=0)
            {
                IsCd = false;//不再走cd
                CurCdTime = cdTime;
            }
        }
            
	}
}
