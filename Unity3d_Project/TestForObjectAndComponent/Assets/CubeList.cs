using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeList : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> object_list ;
    void Start()
    {
        GameObject newOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newOne.transform.localEulerAngles = new Vector3(30, 50, 60);
        newOne.transform.localPosition = new Vector3(1, 2, 3);
        newOne.name = "Cube_1";

        object_list = new List<GameObject>(FindObjectsOfType<GameObject>());
        object_list.Add(newOne);

        GameObject oldOne = GameObject.Find("Cube_1");

        if (oldOne)
        {
            print(oldOne.transform.position);
        }

        GameObject inOne = transform.Find("CubeList_1").Find("CubeList_2").Find("Cube_001").gameObject;

        Destroy(inOne);

        if (inOne)
        {
            print(inOne.transform.position);
        }

        Rigidbody rig = transform.GetComponent<Rigidbody>();
        print(rig.mass);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -10)
        {
            transform.position = new Vector3(0,0,0);
        }
    }
    private void OnMouseDown()
    {

    }
}
