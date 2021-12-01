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

    //��ʾ�˵����UI
    public void ShowMainPanel()
    {
        mainPanel.gameObject.SetActive(true);
        gamePanel.gameObject.SetActive(false);
        gameFilePanel.gameObject.SetActive(false);
    }

    //��ʾ��Ϸҳ��UI
    public void ShowGamePanel()
    {
        mainPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
        gameFilePanel.gameObject.SetActive(false);
    }

    //��ʾ������UI
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
