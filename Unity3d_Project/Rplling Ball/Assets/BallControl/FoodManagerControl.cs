using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManagerControl : MonoBehaviour
{

    //声明游戏对象
    public GameObject foodPrefab;
    public List<GameObject> foodList;
    float position = 1.5f;
    int target_foods = 8;

    void Start()
    {

        //for (int i = 0; i < target_foods; i++)
        //{
        //    while (true)
        //    {
        //        int food_number = (int)Random.Range(0.1f, 2.9f);
        //        foodPrefab = Resources.Load<GameObject>("FoodMode/FoodMode"+food_number);
        //        bool tmp_result = true;
        //        float x = Random.Range(-9.0f, 9.0f);
        //        float y = 0.7f;
        //        float z = Random.Range(-9.0f, 9.0f);

        //        Vector3 new_position = new Vector3(x, y, z);

        //        foreach (GameObject oneFood in foodList)
        //        {
        //            Vector3 food_position = oneFood.transform.position;
        //            Vector3 tmp = new_position - food_position;

        //            if (tmp.magnitude < position)
        //            {
        //                tmp_result = false;
        //                break;
        //            }
        //        }

        //        if (tmp_result)
        //        {
        //            GameObject new_food = Instantiate<GameObject>(foodPrefab);
        //            new_food.transform.SetParent(this.gameObject.transform);
        //            new_food.transform.position = new Vector3(x, y, z);
        //            new_food.GetComponent<FoodControl>().speed = Random.RandomRange(20f, 50f);
        //            foodList.Add(new_food);
        //            break;
        //        }
        //    }
        //}

        //使用协程的方式，将创建食物的流程放在协程之中执行。一定程度有解耦的作用
        StartCoroutine(CreatFoodCoroutine());
    }

    public void deleteFood(GameObject go)
    {
        foodList.Remove(go);
        DestroyObject(go);
    }


    private void Update()
    {
        
    }

    IEnumerator CreatFoodCoroutine()
    {
        yield return null;
        while (true)
        {
            if (foodList.Count < target_foods)
            {
                CreatOneFood();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void CreatOneFood()
    {
        while (true)
        {
            int food_number = (int)Random.Range(0.1f, 2.9f);
            foodPrefab = Resources.Load<GameObject>("FoodMode/FoodMode" + food_number);
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
                new_food.transform.SetParent(this.gameObject.transform);
                new_food.transform.position = new Vector3(x, y, z);
                new_food.GetComponent<FoodControl>().speed = Random.RandomRange(20f, 50f);
                foodList.Add(new_food);
                break;
            }
        }
    }
}
