using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroyer : MonoBehaviour
{
    public float time;

    private float plannedDestructionTime;
    // Start is called before the first frame update
    void Start()
    {
        plannedDestructionTime = Time.time + time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible && Time.time >= plannedDestructionTime)
        {
            Destroy(this.gameObject);
        }
    }
}
