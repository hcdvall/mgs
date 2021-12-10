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
    //delegate List<Node> Pathfinder(Vector3 startPos, Vector3 targetPo);
    //Pathfinder pathfinder;
    public GameObject AStar;
    public GameObject tailPrefab;
    LinkedList<GameObject> tail = new LinkedList<GameObject>();
    bool ate = false;
    private List<Vector3> foodLocations;
    private Vector3 closestFoodPosition;

    private GameObject[] foods;

    void Start()
    {
        Debug.Log("SHEEEEEEY!");
        InvokeRepeating("Stepping", 0.3f, 0.3f);
        //Debug.Log(pathfinding.CurrentBestPath);
        thePath = pathfinding.FindPath(this.transform.position, target.position);
    }

    void Awake() 
    {
        pathfinding = AStar.GetComponent<Pathfinding>();
        //pathfinder = pathfinding.FindPath;
        //thePath = pathfinding.FindPath(this.transform.position, target.position);
        
    }


	void Update() 
    {
        //thePath = pathfinding.FindPath(this.transform.position, target.position);
        //thePath = pathfinder(this.transform.position, target.position);
        //thePath = pathfinder?.Invoke(this.transform.position, target.position);
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
        // Move seeker to the first position in the path
        this.transform.position = thePath.First().worldPosition;
        //Remove the first element in the path
        thePath.RemoveAt(0);

        if (ate) 
        {
            GameObject tailGo = Instantiate(tailPrefab, currentEnemyPos, Quaternion.identity);
            tailGo.GetComponent<Renderer>().material.color =
                new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1f);
            tail.AddFirst(tailGo.gameObject);
            ate = false;
        }
        else if (tail.Count > 0) 
        {

            Vector2 nextPos  = new Vector2();
            Vector2 tailPos  = new Vector2();
            int count = 0;
            foreach ( GameObject go in tail ) 
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
        }
    }

    public void FoodDropped(Vector3 pos)
    {
        foodLocations = new List<Vector3>();
        foods = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foods)
            foodLocations.Add(food.transform.position);

        closestFoodPosition = FindClosestFood(foodLocations);

        thePath = pathfinding.FindPath(this.transform.position, closestFoodPosition);
        //Debug.Log("Path length: " + thePath.Count);
        //Debug.Log("Food Dropped at " + closestFoodPosition);
    }

}