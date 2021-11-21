using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRemoval : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Food") 
        {
            Debug.Log("Food spawned on tail and is therefore removed");
            Destroy(collision.gameObject);
        }
    }
}
