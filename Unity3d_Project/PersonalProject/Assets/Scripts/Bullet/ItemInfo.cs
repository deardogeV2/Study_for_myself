using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int Number;
    void Start()
    {
        Number = Random.Range(30, 120);
    }
}
