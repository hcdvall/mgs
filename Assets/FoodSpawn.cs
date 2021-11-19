using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject foodPrefab;
    public float spawnTime = 1.0f;
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    void Start()
    {
        StartCoroutine(FoodSpawner());
    }

    IEnumerator FoodSpawner()
    {
        while(true)
            {
                yield return new WaitForSeconds(spawnTime);

                int x = (int)Random.Range(borderLeft.position.x+1, borderRight.position.x-1);
                int y = (int)Random.Range(borderBottom.position.y+1, borderTop.position.y-1);
                Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
            }
     }
}
