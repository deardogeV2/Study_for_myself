using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZoom : MonoBehaviour
{
    // ���ռ������
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
