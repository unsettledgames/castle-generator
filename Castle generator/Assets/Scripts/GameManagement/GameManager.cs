using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Rocks and grass data")]
    public int minCliffHeight;
    public int maxCliffHeight;
    public int minCliffWidth;
    public int maxCliffWidth;
    public int nCliffs;

    [Header("Tilesets labels")]
    public Tileset cliffTileSet;

    public GameObject generationParent;

    public void CheckData()
    {
        // Clear all the previous tiles
        for (int i = 0; i < generationParent.transform.childCount; i++)
        {
            Destroy(generationParent.transform.GetChild(i).gameObject);
        }

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
                minCliffHeight = int.Parse(minCh);
                maxCliffHeight = int.Parse(maxCh);
                minCliffWidth = int.Parse(minCw);
                maxCliffWidth = int.Parse(maxCw);

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
        int cliffHeight = UnityEngine.Random.Range(minCliffHeight, maxCliffHeight);
        int cliffWidth = UnityEngine.Random.Range(minCliffWidth, maxCliffWidth);
        Vector3 nextPosition = Camera.main.transform.position;

        for (int i=0; i<nCliffs; i++)
        {
            nextPosition = GenerateCliff(nextPosition, cliffHeight, cliffWidth).bridgePositions[0];
        }
    }

    

    private PossibleContactPoints GenerateCliff(Vector3 start, int height, int width)
    {
        PossibleContactPoints ret = new PossibleContactPoints();

        // Converting start position to int
        int xStart = (int)start.x;
        int yStart = (int)start.y;
        // Crating an int start position
        Vector2 intStartPos = new Vector2(xStart, yStart);

        // Calculating end position
        int xEnd = xStart + width;
        int yEnd = yStart + height;

        // I'm drawing a rectangle of tiles
        for (int x=xStart; x<xEnd ; x++)
        {
            for (int y=yStart; y<yEnd; y++)
            {
                string[] possibleTiles = GetPossibleTiles(xStart, xEnd, yStart, yEnd, x, y);
                string tile = possibleTiles[0];


                if (possibleTiles.Length != 1)
                {
                    int index = UnityEngine.Random.Range(0, possibleTiles.Length);

                    Debug.Log(index);
                    tile = possibleTiles[index];
                }

                Debug.Log(tile);

                GameObject instantiated = Instantiate((GameObject)Resources.Load(tile), intStartPos + new Vector2(x, y), Quaternion.Euler(Vector2.zero));
                instantiated.transform.parent = generationParent.transform;
            }
        }

        return ret;
    }

    /** Returns the right tile depending on the rect coordinates and tiles relative position
     * 
     *  xStart:  x coordinate of the bottom left corner of the rect to generate
     *  xEnd:    x coordinate of the top right corner of the rect to generate
     *  yStart:  y coordinate of the bottom left corner of the rect to generate
     *  xEnd:    y coordinate of the top right corner of the rect to generate
     *  
     *  return: an array of string containing the resource paths of the possible tiles 
     */
    private string[] GetPossibleTiles(int xStart, int xEnd, int yStart, int yEnd, int x, int y)
    {
        // Left tiles
        if (x == xStart)
        {
            // Bottom left tile
            if (y == yStart)
            {
                return cliffTileSet.GetBottomLeft();
            }
            // Top left tile
            else if (y == (yEnd - 1))
            {
                return cliffTileSet.GetTopLeft();
            }
            // Middle left tile
            else
            {
                return cliffTileSet.GetMiddleLeft();
            }
        }
        // Right tiles
        else if (x == (xEnd - 1))
        {
            // Bottom right tile
            if (y == yStart)
            {
                return cliffTileSet.GetBottomRight();
            }
            // Top right tile
            else if (y == (yEnd - 1))
            {
                return cliffTileSet.GetTopRight();
            }
            else
            {
                return cliffTileSet.GetMiddleRight();
            }
        }
        // Middle tiles
        else
        {
            // Bottom middle tile
            if (y == yStart)
            {
                return cliffTileSet.GetBottomMiddle();
            }
            // Top middle tile
            else if (y == (yEnd - 1))
            {
                return cliffTileSet.GetTopMiddle();
            }
            // Middle middle tile
            else
            {
                return cliffTileSet.GetMiddleMiddle();
            }
        }
    }
    public void Generate()
    {
        CheckData();
    }
}
