using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{

    Vector2 dir = Vector2.right;
    private string currentDir = "Rightwards"; 
    List<Transform> tail = new List<Transform>();
    bool ate = false;
    public GameObject tailPrefab;
    public GameObject canvas;
    

    void Start()
    {
        InvokeRepeating("Stepping", 0.1f, 0.1f);
    }

    void Update()
    {
        SetDirection();
    }

    private void SetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (currentDir != "Downwards")
            {
                dir = new Vector2(0, 1);
                currentDir = "Upwards";
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (currentDir != "Upwards")
            {
                dir = new Vector2(0, -1);
                currentDir = "Downwards";
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {   
            if (currentDir != "Rightwards")
            {
                dir = new Vector2(-1, 0);
                currentDir = "Leftwards";
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (currentDir != "Leftwards")
            {
                dir = new Vector2(1, 0);
                currentDir = "Rightwards";
            }
        }
    }

    //private void Stepping()
    //{
    //    this.transform.Translate(dir);
    //}


    void OnTriggerEnter2D(Collider2D coll) 
    {
        // Food?
        if (coll.tag == "Food") 
        {
            //collisionInfo.gameObject.tag == "Enemy"
            // Get longer in next Move call
            ate = true;
        
            // Remove the Food
            Destroy(coll.gameObject);
        }
        else if (coll.tag == "Tail")
        {
            Debug.Log("CUT SNAKE");
             Destroy(coll.gameObject);
        }
        else if (coll.tag == "Border")
        {
            canvas.SetActive(true);
        }
    }


    void Stepping() {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);

        // Ate something? Then insert new Element into gap
        if (ate) {
            // Load Prefab into the world
            GameObject g =(GameObject)Instantiate(tailPrefab,
                                                v,
                                                Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a Tail?
        else if (tail.Count > 0) {
            // Move last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count-1);
        }
        
        
        
        for (int i=0; i < tail.Count; i++)
        {
            //Debug.Log(string.Join(", ", tail));
        }

        foreach(Transform i in tail)
        {
            
        }
        
    }
}
