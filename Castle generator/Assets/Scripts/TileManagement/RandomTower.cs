using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTower : MonoBehaviour
{
    // Tower generation data
    public float minYOffset;
    public float maxYOffset;

    public float minXOffset;
    public float maxXOffset;

    public int minLength;
    public int maxLength;
    
    // Next possible tiles
    public string[] nextTiles;
    // Probability that a tower will be generated
    public float towerProbability = 0;

    private void Start()
    {
        if (Random.Range(0, 100) < towerProbability)
        {
            string tile = nextTiles[Random.Range(0, nextTiles.Length)];
            Vector3 pos = transform.position + new Vector3(Random.Range(minXOffset, maxXOffset), Random.Range(minYOffset, maxYOffset));
            int nPieces = Random.Range(minLength, maxLength);

            for (int i = 0; i < nPieces; i++)
            {
                GameObject instantiated = Instantiate(
                    (GameObject)Resources.Load(tile),
                    pos,
                    Quaternion.Euler(Vector3.zero)
                );
                instantiated.transform.parent = transform;

                if (i == (nPieces - 1))
                {
                    instantiated.GetComponent<InstantiateTileOnAwake>().Instantiate();
                }

                pos = new Vector3(pos.x, pos.y + 1);
            }
        }
    }
}
