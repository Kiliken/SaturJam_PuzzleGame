using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        
    }


    void GenerateGameTile(int size)
    {
        cubes = new GameObject[size, size];
        validPos = new Vector3[size, size];
        cubeUsed = new bool[size, size];

        float startPosX = -(size / 2 - .5f);
        float startPosZ = (size / 2 - .5f);
        Vector3 cursorPos = new Vector3(startPosX, .5f, startPosZ);

        

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
            cursorPos += Vector3.back;
        }
    }

    void RandomSort()
    {
        int piece = 0;

        int x = 0;
        int z = 0;

        do
        {
            x = Random.Range(0, tileSize -1);
            z = Random.Range(0, tileSize - 1);

            if (cubeUsed[x,z] == true)
                continue;
            tilePieces.Add(Instantiate(piecePref));
            MakePiece(tilePieces[piece], x,z);
            piece++;
        }
        while(false);
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
        int size = Random.Range(0, 5);
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
