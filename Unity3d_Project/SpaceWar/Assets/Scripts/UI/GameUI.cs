using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    MainPanel mainPanel;
    GamePanel gamePanel;
    GameFailPanel gameFilePanel;
    GameRulesPanel gameRulesPanel;

    
    //�������塣
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
        //�ڿ�ʼ��ʱ��ֱ�ӽ�������Ϊ������ֵ��instance��
        instance = this;
    }

    
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //��ȡ�����ű��� ʹ�ð�ȫд��
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

        Transform GameFailPanelTsf = transform.Find("GameFailPanel");
        if (GameFailPanelTsf)
        {
            gameFilePanel = GameFailPanelTsf.GetComponent<GameFailPanel>();
            print("��ȡ������Ϸʧ�ܵĶ���");
        }

        Transform GameRulesPanelTsf = transform.Find("GameRulesPanel");
        if (GameRulesPanelTsf)
        {
            gameRulesPanel = GameRulesPanelTsf.GetComponent<GameRulesPanel>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //��ʾ�˵����UI
    public void ShowMainPanel()
    {
        mainPanel.gameObject.SetActive(true);
        gamePanel.gameObject.SetActive(false);
        gameFilePanel.gameObject.SetActive(false);
        gameRulesPanel.gameObject.SetActive(false);
    }

    //��ʾ��Ϸҳ��UI
    public void ShowGamePanel()
    {
        mainPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
        gameFilePanel.gameObject.SetActive(false);
        gameRulesPanel.gameObject.SetActive(false);
    }

    //��ʾ��Ϸʧ��ҳ��
    public void ShowGameFilePanel()
    {
        mainPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        gameFilePanel.gameObject.SetActive(true);
        gameRulesPanel.gameObject.SetActive(false);
    }

    //��ʾ��Ϸ���UI
    public void ShowGameRulesPanel()
    {
        mainPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        gameFilePanel.gameObject.SetActive(false);
        gameRulesPanel.gameObject.SetActive(true);
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
