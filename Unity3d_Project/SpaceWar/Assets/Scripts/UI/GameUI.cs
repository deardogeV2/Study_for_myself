using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    MainPanel mainPanel;
    GamePanel gamePanel;
    GameFilePanel gameFilePanel;

    
    //单例定义。
    private static GameUI instance;
    public static GameUI Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        //在开始的时候直接将本身作为单例赋值给instance。
        instance = this;
    }

    
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //获取三个脚本， 使用安全写法
        Transform MainPanelTsf = transform.Find("MainPanel");
        if (MainPanelTsf)
        {
            mainPanel = MainPanelTsf.GetComponent<MainPanel>();
        }

        Transform GamePanelTsf = transform.Find("GamePanel");
        if (GamePanelTsf)
        {
            gamePanel = GamePanelTsf.GetComponent<GamePanel>();
        }

        Transform GameFilePanelTsf = transform.Find("GameFilePanel");
        if (GameFilePanelTsf)
        {
            gameFilePanel = GameFilePanelTsf.GetComponent<GameFilePanel>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //显示菜单面板UI
    public void ShowMainPanel()
    {
        mainPanel.gameObject.SetActive(true);
        gamePanel.gameObject.SetActive(false);
        gameFilePanel.gameObject.SetActive(false);
    }

    //显示游戏页面UI
    public void ShowGamePanel()
    {
        mainPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
        gameFilePanel.gameObject.SetActive(false);
    }

    //显示简介面板UI
    public void ShowGameFilePanel()
    {
        mainPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        gameFilePanel.gameObject.SetActive(true);
    }

    public void updateLife(int curLife)
    {
        gamePanel.updateLife(curLife);
    }

    public void updatePoint(int curPoint)
    {
        gamePanel.updatePoint(curPoint);
    }
}
