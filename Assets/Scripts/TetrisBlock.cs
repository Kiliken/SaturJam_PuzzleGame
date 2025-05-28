using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TetrisBlock : MonoBehaviour
{

    [SerializeField] private Vector3 rotationPoint;
    private Vector2 _mousePos;

    private float previusTime;
    private float fallTime = 0.8f;

    public static int height = 20;
    public static int width = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        if (Time.time - previusTime > fallTime)
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                transform.position -= Vector3.down;
                this.enabled = false;
                FindAnyObjectByType<SpawnTetramino>().NewTetramino();
            }
                
            previusTime = Time.time;
        }
    }

    void Inputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float y = Input.mousePosition.y - _mousePos.y;
            float x = Input.mousePosition.x - _mousePos.x;
            if (Mathf.Abs(x) < 10 && Mathf.Abs(y) < 10)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                if (!ValidMove())
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
            }
            else if (Mathf.Abs(y) < Mathf.Abs(x * 2))
            {
                if (x > 0)
                {
                    transform.position += Vector3.right;
                    if (!ValidMove())
                        transform.position -= Vector3.right;
                }
                else
                {
                    transform.position += Vector3.left;
                    if (!ValidMove())
                        transform.position -= Vector3.left;
                }

            }
            else
                Debug.Log("It's going down");
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
                return false;
        }

        return true;
    }
}
