using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{   
    private Pathfinding pathfinding;
    public Transform target;
    public List<Node> thePath;
    public GameObject AStar;
    public GameObject tailPrefab;
    LinkedList<GameObject> tail = new LinkedList<GameObject>();
    bool ate = false;
    private List<Vector3> foodLocations;
    public Vector3 closestFoodPosition;

    private GameObject[] foods;


    void Start()
    {
        InvokeRepeating("Stepping", 0.3f, 0.3f);
        thePath = pathfinding.FindPath(this.transform.position, target.position);
    }

    void Awake() 
    {
        pathfinding = AStar.GetComponent<Pathfinding>();
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Food") 
        {
            ate = true;
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
        }
    }

    private Vector3 FindClosestFood(List<Vector3> foodLocations)
    {
        Vector3 tMin = new Vector3();
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        
        if (foodLocations.Count != 0)
        {
            foreach (Vector3 pos in foodLocations)
            {
                float dist = Vector3.Distance(pos, currentPos);
                if (dist < minDist)
                {
                    tMin = pos;
                    minDist = dist;
                }
            }
        } 
        else
        {
            tMin = target.position;
        }
        return tMin;
    }

    public void Stepping()
    {
        Vector2 currentEnemyPos = this.transform.position;
        if (thePath.Count > 0)
        {
            transform.position = thePath.First().worldPosition;
            thePath.RemoveAt(0);
        }

        if (ate) 
        {
            GameObject tailGo = Instantiate(tailPrefab, currentEnemyPos, Quaternion.identity);
            tail.AddFirst(tailGo.gameObject);
            PathRecalculation();
            ate = false;
        }
        else if (tail.Count > 0) 
        {
            Vector2 nextPos  = new Vector2();
            Vector2 tailPos  = new Vector2();
            int count = 0;
            foreach (GameObject go in tail) 
            {   
                if (count == 0)
                {
                    nextPos = go.transform.position; 
                    go.transform.position = currentEnemyPos;
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
            
            //tail.Last().transform.position = currentEnemyPos;
            //tail.AddFirst(tail.Last());
            //tail.RemoveLast();
        }
    }

    public void PathRecalculation()
    {
        foodLocations = new List<Vector3>();
        foods = GameObject.FindGameObjectsWithTag("Food");
        
        foreach (GameObject food in foods)
            foodLocations.Add(food.transform.position);
        
        closestFoodPosition = FindClosestFood(foodLocations);
        
        thePath = pathfinding.FindPath(this.transform.position, closestFoodPosition);
    }
}