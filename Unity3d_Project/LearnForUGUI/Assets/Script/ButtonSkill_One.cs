using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkill_One : MonoBehaviour
{
    // 每一个技能框的脚本

    // Button组件
    Button ButtonSkill; // 按钮对象
    Text CdShowTime; //显示剩余CD时长
    Image ImgMask; //CD时间蒙版
    Text ControlText; //控制按钮显示名称

    // 技能基本属性
    public float CdTime = 3f; //单个技能的CD时间
    private float cdCurTime; // 技能剩余冷却时间
    public KeyCode hotKey = KeyCode.Alpha1; // 按钮值


    // 变化属性封装
    bool isCd;
    public bool IsCd
    {
        get
        {
            return isCd;
        }
        set
        {
            //当进入CD后需要添加的一些行为在这里定义。
            isCd = value;
            CdShowTime.gameObject.SetActive(isCd);
            ImgMask.gameObject.SetActive(isCd);

        }
    }
    public float CdCurTime
    {
        get
        {
            return cdCurTime;
        }
        set
        {
            cdCurTime = value;
            //时间发生变化后控件变换
            CdShowTime.text = cdCurTime.ToString("#0.00");
            ImgMask.fillAmount = CdCurTime / CdTime;


        }
    }

    void Start()
    {
        cdCurTime = CdTime;
        ButtonSkill = GetComponent<Button>();
        CdShowTime = transform.Find("TextCountDown").GetComponent<Text>();
        ImgMask = transform.Find("Mask").GetComponent<Image>();
        ControlText = transform.Find("TextControl").GetComponent<Text>();
        ControlText.text = hotKey.ToString();
        IsCd = false;
        //按钮控制按钮变换逻辑暂时不写
    }

    // Update is called once per frame
    void Update()
    {
        //没CD时，并且按下按钮
        if (IsCd == false)
        {
            if (Input.GetKeyDown(hotKey))
            {
                IsCd = true;
            }
        }
        // CD期间
        if (IsCd == true)
        {
            CdCurTime -= Time.deltaTime;
            if (CdCurTime <= 0)
            {
                IsCd = false;
                CdCurTime = CdTime;
            }
        }
    }
}
