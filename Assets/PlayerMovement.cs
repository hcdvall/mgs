using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Vector2 dir = Vector2.right;
    void Start()
    {
        InvokeRepeating("Stepping", 0.5f, 0.5f);
    }

    void Update()
    {
        SetDirection();
    }

    private void SetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = new Vector2(0, 1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = new Vector2(0, -1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = new Vector2(-1, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = new Vector2(1, 0);
        }
    }

    private void Stepping()
    {
        this.transform.Translate(dir);
    }
}
