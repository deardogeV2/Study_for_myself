using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRulesPanel : MonoBehaviour
{
    Button ButtonBack;
    void Start()
    {
        ButtonBack = transform.Find("ButtonBack").GetComponent<Button>();
        ButtonBack.onClick.AddListener(OnClickButtonBack);
    }

    void Update()
    {
        
    }

    void OnClickButtonBack()
    {
        GameUI.Instance.ShowMainPanel();
    }
}
