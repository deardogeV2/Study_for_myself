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
        //添加监听事件委托——游戏开始
        ButtonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        ButtonStart.onClick.AddListener(OnButtonStart);

        //添加监听事件委托——游戏简介
        ButtonRules = GameObject.Find("ButtonRules").GetComponent<Button>();
        ButtonRules.onClick.AddListener(OnButtonRules);

        //添加监听事件委托——游戏退出
        ButtonExit = GameObject.Find("ButtonExit").GetComponent<Button>();
        ButtonExit.onClick.AddListener(OnButtonExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonStart()
    {
        //点击了开始按钮之后，实际操作：
        //场景切换：
        //  场景加载方法1: 直接加载
        SceneManager.LoadScene("Game");
        // 单例访问。
        GameUI.Instance.ShowGamePanel();
    }

    public void OnButtonRules()
    {
        GameUI.Instance.ShowGameRulesPanel();
    }

    public void OnButtonExit()
    {
        print("点击了开始。");
    }
}
