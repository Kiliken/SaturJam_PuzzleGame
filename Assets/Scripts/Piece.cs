using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-4, 4), Random.Range(-5, -11), 7);
        //se hai voglia fai io coso
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
