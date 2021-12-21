using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


public class Tools
{

    public static Transform findChild(Transform transParent, string childName)
    {
        Transform transChild = transParent.Find(childName);
        if (transChild == null)//如果没有找到
        {
            foreach (Transform t in transParent)//父物体下面的所有子物体
            {
                transChild = findChild(t, childName);
                if (transChild != null)
                {
                    return transChild;
                }
            }
        }
        return transChild;
    }

    //加载图片(对2D游戏使用)
    public static IEnumerator LoadImage(string url, SpriteRenderer render)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return www;
        
        Texture2D texture = www.texture;
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        render.sprite = sp;
    }
    //对UI使用
    public static IEnumerator LoadImage(string url, Image image)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return www;

        Texture2D texture = www.texture;
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = sp;
        image.gameObject.SetActive(true);
    }

    public static void rotateTheTarget(Transform self, Transform target, float speedAngle = 30.0f)
    {
        if (target == null)
        {
            //self.eulerAngles = Vector3.zero;
            return;
        }

        Vector3 pointSelf = self.position;
        Vector3 pointAim = target.position;
        pointSelf.y = 0.0f;
        pointAim.y = 0.0f;
        Vector3 dir = (pointAim - pointSelf).normalized;
        Quaternion destRotation = Quaternion.LookRotation(dir);
        self.rotation = Quaternion.Slerp(self.rotation, destRotation, Time.deltaTime * speedAngle);//插值运算
    }
}

