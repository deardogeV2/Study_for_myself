using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManagerControl : MonoBehaviour
{

    //声明游戏对象
    public GameObject foodPrefab;

    void Start()
    {
        float position = BallManager.position;
        int target_foods = BallManager.target_foods;
        List<GameObject> foodList = BallManager.foodList;
        for (int i = 0; i < target_foods; i++)
        {
            while (true)
            {
                bool tmp_result = true;
                float x = Random.Range(-9.0f, 9.0f);
                float y = 0.7f;
                float z = Random.Range(-9.0f, 9.0f);

                Vector3 new_position = new Vector3(x, y, z);

                foreach (GameObject oneFood in foodList)
                {
                    Vector3 food_position = oneFood.transform.position;
                    Vector3 tmp = new_position - food_position;

                    if (tmp.magnitude < position)
                    {
                        tmp_result = false;
                        break;
                    }
                }

                if (tmp_result)
                {
                    GameObject new_food = Instantiate<GameObject>(foodPrefab);
                    new_food.transform.position = new Vector3(x, y, z);
                    foodList.Add(new_food);
                    break;
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
