using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapBg : MonoBehaviour
{
    Camera playCamera;
    Button miniMapButtonBig;
    Button miniMapButtonSmall;
    void Start()
    {
        playCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        miniMapButtonBig = transform.Find("MiniMapButtonBig").GetComponent<Button>();
        miniMapButtonSmall = transform.Find("MiniMapButtonSmall").GetComponent<Button>();

        miniMapButtonBig.onClick.AddListener(OnMiniMapButtonBig);
        miniMapButtonSmall.onClick.AddListener(OnMiniMapButtonSmall);
    }

    void OnMiniMapButtonBig()
    {
        if (playCamera.orthographicSize > 1)
        {
            playCamera.orthographicSize -= 0.5f;
        }
    }
    void OnMiniMapButtonSmall()
    {
        if (playCamera.orthographicSize < 4)
        {
            playCamera.orthographicSize += 0.5f;
        }
    }
}
