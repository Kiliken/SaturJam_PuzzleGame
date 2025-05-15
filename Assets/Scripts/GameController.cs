using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject cubePref;

    [SerializeField]
    GameObject piecePref;

    [SerializeField]
    Transform tile;

    [SerializeField]
    int tileSize;

    private GameObject[,] cubes;
    private Vector3[,] validPos;
    private bool[,] cubeUsed;

    private Vector3 screenPos;
    private Vector3 something;

    private List<GameObject> tilePieces = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateGameTile(tileSize);
        RandomSort();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {

            /*screenPos = Input.mousePosition;
            screenPos.z = -Camera.main.transform.position.z;
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(screenPos), Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name + " has been hit");
                if (hit.collider.gameObject.tag == "Piece")
                {

                }
            }

            */
        }
    }


    void GenerateGameTile(int size)
    {
        cubes = new GameObject[size, size];
        validPos = new Vector3[size, size];
        cubeUsed = new bool[size, size];

        float startPosX = -(size / 2 - .5f);
        float startPosY = size / 2 - .5f;
        Vector3 cursorPos = new Vector3(startPosX, startPosY, .5f);

        

        // Set a random color in the MaterialPropertyBlock
        


        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                validPos[i, j] = cursorPos;
                cubes[i, j] = Instantiate(cubePref, tile);
                cubes[i, j].transform.position = cursorPos;
                cursorPos += Vector3.right;

                
            }

            cursorPos.x = startPosX;
            cursorPos += Vector3.down;
        }
    }

    void RandomSort()
    {
        int piece = 0;

        for (int i = 0; i < tileSize; i++)
        {
            for (int j= 0; j < tileSize; j++)
            {
                if (cubeUsed[i, j] == true)
                    continue;
                tilePieces.Add(Instantiate(piecePref, validPos[i,j], Quaternion.identity));
                MakePiece(tilePieces[piece], i, j);
                piece++;
            }
        }
    }

    void MakePiece(GameObject pieceCenter, int posX, int posZ)
    {
        //MAKE THIS IN RANDOM SORT
        if (cubeUsed[posX, posZ] == true) {
            Destroy(pieceCenter);
            tilePieces.Remove(pieceCenter);
            return;
        }

        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetColor("_Color", Random.ColorHSV());
        cubes[posX, posZ].GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);

        cubes[posX, posZ].transform.parent = pieceCenter.transform;
        cubeUsed[posX, posZ] = true;
        int size = Random.Range(3, 6);
        for (int i = 0; i< size; i++)
        {
            int[] dir = RandomDirection(posX, posZ);

            if (cubeUsed[dir[0], dir[1]] == true)
                continue;

            cubes[dir[0], dir[1]].transform.parent = pieceCenter.transform;
            cubes[dir[0], dir[1]].GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
            cubeUsed[dir[0], dir[1]] = true;
        }

        
    }

    int[] RandomDirection(int a, int b)
    {
        int x  = a;
        int z  = b;
        switch (Random.Range(0, 3))
        {
            case 0:
                z += 1;
                break;
            case 1:
                z -= 1;
                break;
            case 2:
                x += 1;
                break;
            case 3:
                x -= 1;
                break;
        }

        if (x < 0 || z < 0 || x > tileSize - 1 || z > tileSize - 1)
            return new int[] { a, b };
        return new int[] { x, z };
    }

    bool FreeCubeCheck() {
        int test = 0;
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                if (cubeUsed[i,j] == false)
                    test++;
            }
        }
        return (test <= tileSize*tileSize);
    }
}
