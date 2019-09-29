using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBigTower : MonoBehaviour
{
    public string[] nextRight;
    public string[] nextLeft;

    public int minLength;
    public int maxLength;

    public float minXNoise;
    public float maxXNoise;
    public float minYNoise;
    public float maxYNoise;

    public float towerProbability = 50;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 100) < towerProbability)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(minXNoise, maxXNoise), Random.Range(minYNoise, maxYNoise));
            int index = Random.Range(0, nextRight.Length);
            int length = Random.Range(GameManager.minTowerLength, GameManager.maxTowerLength);

            for (int i = 0; i < length; i++)
            {
                GameObject tmp = Instantiate(
                    (GameObject)Resources.Load(nextLeft[index]),
                    pos + new Vector3(0, i),
                    Quaternion.Euler(Vector3.zero)
                );

                if (i == (length - 1))
                {
                    tmp.GetComponent<InstantiateTileOnAwake>().Instantiate();
                }

                tmp = Instantiate(
                    (GameObject)Resources.Load(nextRight[index]),
                    pos + new Vector3(1, i),
                    Quaternion.Euler(Vector3.zero)
                );

                if (i == (length - 1))
                {
                    tmp.GetComponent<InstantiateTileOnAwake>().Instantiate();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
