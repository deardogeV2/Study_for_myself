using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZoom : MonoBehaviour
{
    // 出空间就销毁

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
