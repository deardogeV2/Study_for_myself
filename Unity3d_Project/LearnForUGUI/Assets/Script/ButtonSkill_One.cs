using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkill_One : MonoBehaviour
{
    // ÿһ�����ܿ�Ľű�

    // Button���
    Button ButtonSkill; // ��ť����
    Text CdShowTime; //��ʾʣ��CDʱ��
    Image ImgMask; //CDʱ���ɰ�
    Text ControlText; //���ư�ť��ʾ����

    // ���ܻ�������
    public float CdTime = 3f; //�������ܵ�CDʱ��
    private float cdCurTime; // ����ʣ����ȴʱ��
    public KeyCode hotKey = KeyCode.Alpha1; // ��ťֵ


    // �仯���Է�װ
    bool isCd;
    public bool IsCd
    {
        get
        {
            return isCd;
        }
        set
        {
            //������CD����Ҫ��ӵ�һЩ��Ϊ�����ﶨ�塣
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
            //ʱ�䷢���仯��ؼ��任
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
        //��ť���ư�ť�任�߼���ʱ��д
    }

    // Update is called once per frame
    void Update()
    {
        //ûCDʱ�����Ұ��°�ť
        if (IsCd == false)
        {
            if (Input.GetKeyDown(hotKey))
            {
                IsCd = true;
            }
        }
        // CD�ڼ�
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
