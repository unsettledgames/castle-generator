using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Rocks and grass data")]
    public float minCliffHeight;
    public float maxCliffHeight;
    public float minCliffWidth;
    public float maxCliffWidth;
    public int nCliffs;

    public GameObject generationParent;

    public void CheckData()
    {
        // Getting string values from input fields
        string minCh = GameObject.FindGameObjectWithTag("MinCliffHeight").GetComponent<InputField>().text;
        string maxCh = GameObject.FindGameObjectWithTag("MaxCliffHeight").GetComponent<InputField>().text;
        string minCw = GameObject.FindGameObjectWithTag("MinCliffWidth").GetComponent<InputField>().text;
        string maxCw = GameObject.FindGameObjectWithTag("MaxCliffWidth").GetComponent<InputField>().text;

        // Checking if they're correct
        if (!minCh.Equals("") && !maxCh.Equals("") && !minCw.Equals("") && !maxCw.Equals(""))
        {
            try
            {
                minCliffHeight = float.Parse(minCh);
                maxCliffHeight = float.Parse(maxCh);
                minCliffWidth = float.Parse(minCw);
                maxCliffWidth = float.Parse(maxCw);

                try
                {
                    nCliffs = (int)GameObject.FindGameObjectWithTag("NCliffs").GetComponent<Slider>().value;
                }
                catch(NullReferenceException e)
                {
                    return;
                }
                

                Debug.Log(minCliffHeight + "," + maxCliffHeight);
                Debug.Log(minCliffWidth + "," + maxCliffWidth);

                if (minCliffHeight > maxCliffHeight || minCliffWidth > maxCliffWidth)
                {
                    // TODO: log error
                    Debug.Log("Check ur math");
                    return;
                }
            }
            catch (Exception e)
            {
                // TODO: log error
                Debug.Log("Dafuq");
                return;
            }
        }

        Debug.Log("Aight correct input");
        // If I'm here, the input is correct, so I can generate the castle
        ActuallyGenerate();
    }

    public void ActuallyGenerate()
    {
        float cliffHeight = UnityEngine.Random.Range(minCliffHeight, maxCliffHeight);
        float cliffWith = UnityEngine.Random.Range(minCliffWidth, maxCliffWidth);
        Vector3 nextPosition = Camera.main.transform.position;

        for (int i=0; i<nCliffs; i++)
        {
            nextPosition = GenerateCliff(nextPosition, cliffHeight, cliffWith);
        }
    }

    private Vector3 GenerateCliff(Vector3 start, float min, float max)
    {
        return Camera.main.transform.position;
    }

    public void Generate()
    {
        CheckData();
    }
}
