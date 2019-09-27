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

    [Header("Bridges data")]
    public int minBridgeWidth;
    public int maxBridgeWidth;

    public int nCliffs;

    [Header("Tilesets labels")]
    public Tileset cliffTileSet;
    public Tileset[] bridgeTilesets;

    public GameObject generationParent;

    // The tileset I'm currently using
    private Tileset currentTileset;

    public void CheckData()
    {
        // Clear all the previous tiles
        for (int i = 0; i < generationParent.transform.childCount; i++)
        {
            Destroy(generationParent.transform.GetChild(i).gameObject);
        }

        // Getting string values from input fields
        // Cliffs
        string minCh = GameObject.FindGameObjectWithTag("MinCliffHeight").GetComponent<InputField>().text;
        string maxCh = GameObject.FindGameObjectWithTag("MaxCliffHeight").GetComponent<InputField>().text;
        string minCw = GameObject.FindGameObjectWithTag("MinCliffWidth").GetComponent<InputField>().text;
        string maxCw = GameObject.FindGameObjectWithTag("MaxCliffWidth").GetComponent<InputField>().text;

        // Bridges
        string minBw = GameObject.FindGameObjectWithTag("MinBridgeWidth").GetComponent<InputField>().text;
        string maxBw = GameObject.FindGameObjectWithTag("MaxBridgeWidth").GetComponent<InputField>().text;

        // Checking if they're correct
        try
        {
            minCliffHeight = int.Parse(minCh);
            maxCliffHeight = int.Parse(maxCh);
            minCliffWidth = int.Parse(minCw);
            maxCliffWidth = int.Parse(maxCw);

            minBridgeWidth = int.Parse(minBw);
            maxBridgeWidth = int.Parse(maxBw);

            try
            {
                nCliffs = (int)GameObject.FindGameObjectWithTag("NCliffs").GetComponent<Slider>().value;
            }
            catch(NullReferenceException e)
            {
                return;
            }
                
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

        Debug.Log("Aight correct input");
        // If I'm here, the input is correct, so I can generate the castle
        ActuallyGenerate();
    }

    public void ActuallyGenerate()
    {
        int cliffHeight = UnityEngine.Random.Range(minCliffHeight, maxCliffHeight);
        int cliffWidth = UnityEngine.Random.Range(minCliffWidth, maxCliffWidth);
        CliffGenerationData startData = new CliffGenerationData();
        bool mustBuildBridge;

        startData.bridgePositions.Add(Camera.main.transform.position);

        for (int i=0; i<nCliffs; i++)
        {
            if (startData.CanChangeHeight())
            {
                cliffHeight = UnityEngine.Random.Range(startData.prevHeight == 0 ? minCliffHeight : startData.prevHeight, maxCliffHeight);
                cliffWidth = UnityEngine.Random.Range(minCliffWidth, maxCliffWidth);
            }
            
            if (i != nCliffs - 1)
            {
                mustBuildBridge = true;
            }
            else
            {
                mustBuildBridge = false;
            }

            startData = GenerateCliff(startData, cliffHeight, cliffWidth, mustBuildBridge);
        }
    }

    private CliffGenerationData GenerateCliff(CliffGenerationData data, int height, int width, bool mustBuildBridge)
    {
        Vector3 start = data.bridgePositions[0];
        Debug.Log("Start position: " + start);
        CliffGenerationData ret = new CliffGenerationData();

        // Converting start position to int
        int xStart = (int)start.x;
        int yStart = (int)start.y;
        // Crating an int start position
        Vector2 intStartPos = new Vector2(xStart, yStart);

        Debug.Log("IntStart: " + intStartPos);

        // Calculating end position
        int xEnd = xStart + width;
        int yEnd = yStart + height;

        // I'm drawing a rectangle of tiles
        for (int x=xStart; x<xEnd ; x++)
        {
            for (int y=yStart; y<yEnd; y++)
            {
                // Instantiated tile
                GameObject instantiated;
                // Using the default tileset
                currentTileset = cliffTileSet;

                // If I have to build a bridge, the cliff is high enough and I'm in the bottom right corner, I build a bridge
                if (mustBuildBridge && (x == (xEnd - 1)) && y == yStart && height >= 3)
                {
                    // Calculating bridge width
                    int bridgeWidth = UnityEngine.Random.Range(minBridgeWidth, maxBridgeWidth);
                    // Calculating start and end points
                    int bridgeStartX = x;
                    int bridgeEndX = x + bridgeWidth;
                    int bridgeStartY = yStart;
                    int bridgeEndY = yEnd;
                    string prefix;

                    // Choosing a random bridge tileset
                    currentTileset = bridgeTilesets[UnityEngine.Random.Range(0, bridgeTilesets.Length)];
                    prefix = currentTileset.prefix;

                    // If I didn't pick a big bridge, I can make it shorter than the cliff
                    if (!((prefix.Contains("/LightBridge/")) || prefix.Contains("/DarkBridge/")))
                    {
                        bridgeEndY = UnityEngine.Random.Range(bridgeStartY, yEnd);
                    }

                    /* Used for those bridges that have 2 top tiles (one above the other one). Since they're added
                     automatically, I have to stop 1 y unit earlier.*/
                    if ((prefix.Contains("/LightBridge/")) || prefix.Contains("/DarkBridge/"))
                    {
                        bridgeEndY -= 1;
                    }

                    // Building the bridge
                    for (int xBridge=x; xBridge < (x + bridgeWidth); xBridge++)
                    {
                        for (int yBridge=yStart; yBridge < bridgeEndY; yBridge++)
                        {
                            // Little bridges don't have a top tile, so I don't instantiate it
                            if ((currentTileset.prefix.Contains("Little") && !(yBridge == (yEnd - 1)) || !currentTileset.prefix.Contains("Little")))
                            {
                                // Position used to instantiate the tile
                                Vector2 instantiationPos = new Vector2(xBridge, yBridge);
                                string[] possibleTiles = GetPossibleTiles(
                                    bridgeStartX, bridgeEndX, bridgeStartY, bridgeEndY, xBridge, yBridge
                                );

                                Debug.Log(possibleTiles[0]);

                                /* The bottom right corner of the bridge is the position of the next 
                                 * cliff, so I add it to the bridgePositions list */
                                if (possibleTiles[0].Contains("BottomRight"))
                                {
                                    Vector2 toAdd = new Vector2(instantiationPos.x, instantiationPos.y);
                                    Debug.Log("Next position: " + toAdd);
                                    ret.bridgePositions.Add(toAdd);
                                    // Also, since the bridge contains part of the cliff, I don't need to draw that part
                                    ret.mustDrawLeft = false;
                                    // Saving bridge type
                                    ret.bridgeType = currentTileset.prefix;
                                    // Adding current Y
                                    ret.prevHeight = bridgeEndY - bridgeStartY;
                                }

                                // Instantiating the tile
                                instantiated = Instantiate(
                                    (GameObject)Resources.Load(possibleTiles[0]),
                                    instantiationPos,
                                    Quaternion.Euler(Vector3.zero)
                                    );

                                instantiated.transform.parent = generationParent.transform;

                                // If I have alternatives, that means I have to add the top part of the bridge
                                if (possibleTiles.Length > 1)
                                {
                                    instantiationPos = new Vector2(instantiationPos.x, instantiationPos.y + 1);
                                    GameObject tmp = Instantiate(
                                        (GameObject)Resources.Load(possibleTiles[1]),
                                        instantiationPos,
                                        Quaternion.Euler(Vector3.zero)
                                        );

                                    tmp.transform.parent = generationParent.transform;
                                }
                            }
                            
                        }
                    }
                }
                else
                {
                    if (((x == xStart) && data.mustDrawLeft) || x != xStart || data.prevHeight <= y)
                    {
                        string[] possibleTiles = GetPossibleTiles(xStart, xEnd, yStart, yEnd, x, y);
                        string tile = possibleTiles[0];

                        if (possibleTiles.Length != 1)
                        {
                            int index = UnityEngine.Random.Range(0, possibleTiles.Length);
                            tile = possibleTiles[index];
                        }

                        Debug.Log(tile);

                        instantiated = Instantiate((GameObject)Resources.Load(tile), new Vector2(x, y), Quaternion.Euler(Vector2.zero));
                        instantiated.transform.parent = generationParent.transform;
                    }
                }
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
                return currentTileset.GetBottomLeft();
            }
            // Top left tile
            else if (y == (yEnd - 1))
            {
                return currentTileset.GetTopLeft();
            }
            // Middle left tile
            else
            {
                return currentTileset.GetMiddleLeft();
            }
        }
        // Right tiles
        else if (x == (xEnd - 1))
        {
            // Bottom right tile
            if (y == yStart)
            {
                return currentTileset.GetBottomRight();
            }
            // Top right tile
            else if (y == (yEnd - 1))
            {
                return currentTileset.GetTopRight();
            }
            else
            {
                return currentTileset.GetMiddleRight();
            }
        }
        // Middle tiles
        else
        {
            // Bottom middle tile
            if (y == yStart)
            {
                return currentTileset.GetBottomMiddle();
            }
            // Top middle tile
            else if (y == (yEnd - 1))
            {
                return currentTileset.GetTopMiddle();
            }
            // Middle middle tile
            else
            {
                return currentTileset.GetMiddleMiddle();
            }
        }
    }
    public void Generate()
    {
        CheckData();
    }
}
