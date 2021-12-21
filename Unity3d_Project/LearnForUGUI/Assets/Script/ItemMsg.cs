using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary> 
/// 物品信息提示
/// </summary>
public class ItemMsg : MonoBehaviour {
    private Text content;//需要显示文本（显示物品描述信息）
    private CanvasGroup cg;
    public float finalAlpha=0.0f;//最终显示的alpha
    private float speedAlpha = 4.0f;
	void Awake()
	{
        content = transform.Find("Content").GetComponent<Text>();
        cg = transform.GetComponent<CanvasGroup>();
	}
	
	void Update () 
	{
        //控制提示信息框的淡入淡出效果
		if(finalAlpha!=cg.alpha)
        {
            cg.alpha = Mathf.Lerp(cg.alpha,finalAlpha,speedAlpha*Time.deltaTime);
            if (Mathf.Abs(finalAlpha - cg.alpha) <= 0.01)
            {
                cg.alpha = finalAlpha;
                
            }
        }
	}
    /// <summary>
    /// 物品提示信息显示
    /// </summary>
    /// <param name="str"></param>
    public void show(string str)
    {
        content.text = str;
        finalAlpha = 1.0f;
    }
    /// <summary>
    /// 物品提示信息隐藏
    /// </summary>
    public void hide()
    {
        finalAlpha = 0;

    }
}
