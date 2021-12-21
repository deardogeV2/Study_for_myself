using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMain : MonoBehaviour 
{
    Button btnBig;
    Button btnSmall;
    Camera minimapCamera;
	void Start ()
	{
        btnBig = transform.Find("TopRight/MinimapBg/BtnBig").GetComponent<Button>();
        btnSmall = transform.Find("TopRight/MinimapBg/BtnSmall").GetComponent<Button>();
        minimapCamera=GameObject.Find("CameraMinimap").GetComponent<Camera>();
        btnBig.onClick.AddListener(OnBtnBig);
        btnSmall.onClick.AddListener(OnBtnSmall);
    }
	void OnBtnBig()
    {
        minimapCamera.orthographicSize -= 0.2f;
        minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize, 3.2f, 5.0f);
    }
    void OnBtnSmall()
    {
        minimapCamera.orthographicSize += 0.2f;
        minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize, 3.2f, 5.0f);
    }
    void Update () 
	{
		
	}
}
