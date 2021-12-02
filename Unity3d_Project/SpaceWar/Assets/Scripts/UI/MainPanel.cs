using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour
{
    Button ButtonStart;
    Button ButtonRules;
    Button ButtonExit;
    void Start()
    {
        //��Ӽ����¼�ί�С�����Ϸ��ʼ
        ButtonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        ButtonStart.onClick.AddListener(OnButtonStart);

        //��Ӽ����¼�ί�С�����Ϸ���
        ButtonRules = GameObject.Find("ButtonRules").GetComponent<Button>();
        ButtonRules.onClick.AddListener(OnButtonRules);

        //��Ӽ����¼�ί�С�����Ϸ�˳�
        ButtonExit = GameObject.Find("ButtonExit").GetComponent<Button>();
        ButtonExit.onClick.AddListener(OnButtonExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonStart()
    {
        //����˿�ʼ��ť֮��ʵ�ʲ�����
        //�����л���
        //  �������ط���1: ֱ�Ӽ���
        SceneManager.LoadScene("Game");
        // �������ʡ�
        GameUI.Instance.ShowGamePanel();
    }

    public void OnButtonRules()
    {
        GameUI.Instance.ShowGameRulesPanel();
    }

    public void OnButtonExit()
    {
        print("����˿�ʼ��");
    }
}
