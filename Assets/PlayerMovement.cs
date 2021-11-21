using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 dir = Vector2.right;
    private string currentDir = "Rightwards"; 
    LinkedList<GameObject> tail = new LinkedList<GameObject>();
    //LinkedListNode test = new ;
    bool ate = false;
    public GameObject tailPrefab;
    public GameObject canvas;
    private float initialSpeed = 0.1f;
    public float powerUp = 0.0f;
    private bool powerUpChange = false;
    

    void Start()
    {
        InvokeRepeating("Stepping", initialSpeed, initialSpeed);
    }

    void Update()
    {
        if (powerUpChange == true) 
        {
            InvokeRepeating("Stepping", initialSpeed + powerUp, initialSpeed + powerUp);
            powerUpChange = false;
        }
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

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Food") 
        {
            ate = true;
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Tail" | collision.tag == "Border")
        {
           CancelInvoke();
           canvas.SetActive(true);
        }
        else if (collision.tag == "PowerUp")
        {
            powerUp += 0.1f;
            powerUpChange = true;
            Destroy(collision.gameObject);
        }
    }

        /*
        I tried out two approaches of updating the tail of the snake
        In both cases I utilize the space/position that the head leaves behind after moving
            
            1. The Stepping procedure starts with saving  the current position of the head
            2. Then the head takes a step (translate) in a set direction (leaving its previous position up for grabs)
            3. If the head collides with food, the boolean "ate" will be set to true
                - A new tail object is instantiated from a prefab on the empty space the head leaved behind
                - The instantiated tail object is added to the first position in a LinkedList called tail
                - Boolean "ate" is set to false again
            
            There is no need to move around objects of the tail (update positions) when food is added to the length of the snake
            But every step when the head does not collide with food the linked list should be updated to move with the head
            
            4. If no collision is observed, I check if the tail is "empty" or not
            5. If the tail contains tail objects I've tried two approaches:
                A. Changing the position of the first object in the list to the heads old position (the gap)
                   And then change the position of all other objects to the position before them
                B. Changing the last objects position to the heads old position
                   And move it to the first position in the linked list
                   Then remove the last object in the list

            Approach B is very neat and effective. 
            But I preferred the idea to keep track of every object in approach A, 
            in case further abilities would be added to tail objects
        */
    void Stepping() 
    {
        Vector2 currentPlayerPos = this.transform.position;
        this.transform.Translate(dir);
    
        if (ate) 
        {
            GameObject tailGo = Instantiate(tailPrefab, currentPlayerPos, Quaternion.identity);
            tailGo.GetComponent<Renderer>().material.color =
                new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1f);
            tail.AddFirst(tailGo.gameObject);
            ate = false;
        }
        else if (tail.Count > 0) {
            Vector2 nextPos  = new Vector2();
            Vector2 tailPos  = new Vector2();
            int count = 0;
            foreach ( GameObject go in tail ) 
            {   
                if (count == 0)
                {
                    nextPos = go.transform.position; 
                    go.transform.position = currentPlayerPos;
                    count += 1;
                }
                else 
                {
                    tailPos = go.transform.position;
                    go.transform.position = nextPos;
                    nextPos = tailPos;
                    count += 1;
                }  
            }

            //tail.Last().transform.position = currentPlayerPos;
            //tail.AddFirst(tail.Last());
            //tail.RemoveLast();
        } 
    }
}
