using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CloudManager : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(minSpeed, maxSpeed), 0);
    }
}
