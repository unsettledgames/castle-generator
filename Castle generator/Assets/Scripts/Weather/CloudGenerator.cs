using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudGenerator : MonoBehaviour
{
    public float cloudIntensity = 50;
    public GameObject[] clouds;
    public int minSortingLayer = -1000;
    public int maxSortingLayer = -990;
    public Slider slider;

    private BoxCollider2D collider;
    private float topY;
    private float bottomY;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        bottomY = -collider.size.y / 2;
        topY = collider.size.y / 2;

        StartCoroutine(GenerateClouds());
    }

    private IEnumerator GenerateClouds()
    {
        while (true)
        {
            if (Random.Range(0, 100) < cloudIntensity)
            {
                GenerateSingleCloud();
            }

            yield return null;
        }
    }

    private void GenerateSingleCloud()
    {
        Vector3 cloudPos = new Vector3(
            transform.position.x,
            Random.Range(transform.position.y + bottomY, transform.position.y + topY)
            );
        int cloudIndex = Random.Range(0, clouds.Length);

        GameObject instantiated = Instantiate(
            clouds[cloudIndex],
            cloudPos,
            Quaternion.Euler(Vector3.zero)
        );
        instantiated.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(minSortingLayer, maxSortingLayer);
    }

    public void SetIntensity()
    {
        cloudIntensity = slider.value;
    }
}
