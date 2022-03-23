using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonStart : EventTrigger
{
    Sprite imageDown;
    Sprite imageUp;
    Image image;

    GameObject GameUI;
    GameObject Player;
    GameObject MiniMap;
    GameObject TopCamera;
    GameObject Game;
    void Start()
    {
        image = transform.GetComponent<Image>();
        imageDown = Resources.Load("UI/Tutorial/UI/ButtonStartDown", typeof(Sprite)) as Sprite;
        imageUp = image.sprite;

        GameUI = GameObject.Find("GameUI");
        MiniMap = GameUI.transform.Find("MiniMapCamera").gameObject;
        TopCamera = GameUI.transform.Find("/TopCamera").gameObject;
        Player = GameUI.transform.Find("/GamePlayer").Find("Player").gameObject;
        Game = GameUI.transform.Find("Canvas").transform.Find("Game").gameObject;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = imageDown;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = imageUp;

        initGame(1);
    }

    public void initGame(int level = 1){
        TopCamera.SetActive(false);
        Player.SetActive(true);
        MiniMap.SetActive(true);
        Game.SetActive(true);

        GameObject.Find("Tutorial").SetActive(false);

        TaskManager task = GameObject.Find("Map").GetComponent<TaskManager>();
        task.changeTaskNumber = level;
        task.shouldStart = true;
    }



}
