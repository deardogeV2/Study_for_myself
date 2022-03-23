using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    Scrollbar scrollbar;
    public float scrollSpeed = 0.1f;
    void Start()
    {
        scrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollbar.value > 0)
        {
            scrollbar.value -= Time.deltaTime*scrollSpeed;
            scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
        }
    }
}
