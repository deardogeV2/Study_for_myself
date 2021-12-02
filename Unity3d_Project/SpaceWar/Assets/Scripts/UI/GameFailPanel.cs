using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFailPanel : MonoBehaviour
{
    //分数
    Text Point;

    //按钮功能
    Button ButtonBackMain;
    Button ButtonAgain;
    void OnEnable()
    {
        Point = transform.Find("Point").GetComponent<Text>();
        Point.text = Player.Instance.CurPoint.ToString();
    }

    void Start()
    {
        ButtonBackMain = transform.Find("ButtonBackMain").GetComponent<Button>();
        ButtonAgain = transform.Find("ButtonAgain").GetComponent<Button>();

        if (ButtonBackMain)
        {
            ButtonBackMain.onClick.AddListener(OnClickButtonBackToMain);
        }
        if (ButtonAgain)
        {
            ButtonAgain.onClick.AddListener(OnClickButtonAgain);
        }
    }

    void OnClickButtonBackToMain()
    {
        GameUI.Instance.ShowMainPanel();
    }

    void OnClickButtonAgain()
    {
        SceneManager.LoadScene("Game");
        GameUI.Instance.ShowGamePanel();
    }

}
