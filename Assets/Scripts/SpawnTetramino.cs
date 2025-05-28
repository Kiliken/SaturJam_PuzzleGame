using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetramino : MonoBehaviour
{
    [SerializeField] GameObject[] tetraminos;

    // Start is called before the first frame update
    void Start()
    {
        NewTetramino();
    }

    public void NewTetramino()
    {
        Instantiate(tetraminos[Random.Range(0, tetraminos.Length)], transform.position, Quaternion.identity);
    }
}
