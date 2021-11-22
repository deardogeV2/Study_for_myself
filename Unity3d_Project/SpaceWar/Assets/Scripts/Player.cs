using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rig;
    float speed = 1f;

    void Start()
    {
      rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (rig)
        {
            rig.velocity = new Vector3(h, 0, v) * speed;

            transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z<30& transform.localRotation.z>-30? transform.localRotation.z+h: transform.localRotation.z));
        }

        if (h == 0f)
        {
            if (transform.rotation.z > 0)
            {
                rig.angularVelocity = new Vector3(0, 0, -30) * 1f;
            }
            else if (transform.rotation.z < 0)
            {
                rig.angularVelocity = new Vector3(0, 0, 30) * 1f;
            }
        }
    }
}
