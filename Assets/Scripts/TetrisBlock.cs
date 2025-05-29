using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TetrisBlock : MonoBehaviour
{

    [SerializeField] private Vector3 rotationPoint;
    private Vector2 _mousePos;
    bool _mouseDown = false;
    bool _isRotate = true;

    private float previusTime;
    private float fallTime = 0.2f;

    public static int height = 20;
    public static int width = 10;

    private static Transform[,] grid = new Transform[width, height];

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
                AddToGrid();
                CheckForLines();

                this.enabled = false;
                FindAnyObjectByType<SpawnTetramino>().NewTetramino();
            }
                
            previusTime = Time.time;
        }
    }

    /*
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
    }*/
    void Inputs()
    {
        float y;
        float x;
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Input.mousePosition;
            _mouseDown = true;
            _isRotate = true;
        }

        if (_mouseDown)
        {
            y = Input.mousePosition.y - _mousePos.y;
            x = Input.mousePosition.x - _mousePos.x;
            if (Mathf.Abs(y) + 100 < Mathf.Abs(x * 2))
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
                _mousePos = Input.mousePosition;
                _isRotate = false;
            }
            if (Mathf.Abs(x) + 200 < Mathf.Abs(y * 2) && y < 0)
            {
                transform.position += Vector3.down;
                if (!ValidMove())
                    transform.position -= Vector3.down;
                
                _mousePos = Input.mousePosition;
                _isRotate = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;
            y = Input.mousePosition.y - _mousePos.y;
            x = Input.mousePosition.x - _mousePos.x;
            if (_isRotate)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                if (!ValidMove())
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
            }
            
        }
    }

    void CheckForLines()
    {
        for (int i = height -1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= Vector3.up;
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
            if (roundedY > 15)
            {
                SceneManager.LoadScene("MainScene");
                return;
            }
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

            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }
}
