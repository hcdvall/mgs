using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject powerUpPrefab;
    public float foodSpawnTime = 3.0f;
    public float powerUpSpawnTime = 15.0f;
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;


    public Event foodDrop;
    ObjectPooler objectPooler;
    void Start()
    {
        //StartCoroutine(FoodSpawner());
        InvokeRepeating("FoodSpawner", foodSpawnTime, foodSpawnTime);
        StartCoroutine(PowerUpSpawner());
    }


    /*
    IEnumerator FoodSpawner()
    {
        while(true)
            {
                yield return new WaitForSeconds(foodSpawnTime);

                int x = (int)Random.Range(borderLeft.position.x+1, borderRight.position.x-1);
                int y = (int)Random.Range(borderBottom.position.y+1, borderTop.position.y-1);
                Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
            }
    }*/

    void FoodSpawner()
    {
        int x = (int)Random.Range(borderLeft.position.x+1, borderRight.position.x-1);
        int y = (int)Random.Range(borderBottom.position.y+1, borderTop.position.y-1);
        ObjectPooler.Instance.SpawnFromPool("Food", new Vector3(x, y, 0), Quaternion.identity); 

        foodDrop.Occured(new Vector3(x, y, 0)); 
    }

    IEnumerator PowerUpSpawner()
    {
        while(true)
            {
                yield return new WaitForSeconds(powerUpSpawnTime);

                int x = (int)Random.Range(borderLeft.position.x+1, borderRight.position.x-1);
                int y = (int)Random.Range(borderBottom.position.y+1, borderTop.position.y-1);
                Instantiate(powerUpPrefab, new Vector2(x, y), Quaternion.identity);
            }
    }
}
