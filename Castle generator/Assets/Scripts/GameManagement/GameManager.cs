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

    public int grassHeight;
    public GameObject grassMiddle;
    public GameObject flowers;
    public GameObject grassIrregularities;

    [Header("Bridges data")]
    public int minBridgeWidth;
    public int maxBridgeWidth;

    [Header("Bastions data")]
    public int minBastionWidth;
    public int maxBastionWidth;
    public int minBastionHeight;
    public int maxBastionHeight;
    

    [Header("Cliff and bridges tilesets")]
    public Tileset cliffTileSet;
    public Tileset[] bridgeTilesets;

    [Header("Towers data")]
    public static int minTowerLength;
    public static int maxTowerLength;

    [Header("Wall tileset")]
    public Tileset wallTileset;

    [Header("Bastion tilesets")]
    public BastionTileset[] bastionTilesets;

    [Header("Other")]
    public GameObject generationParent;


    // The tileset I'm currently using
    private Tileset currentTileset;
    // The bastion tileset I'm currently using
    private BastionTileset currentBastionTileset;

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

        // Bastions
        string minBah = GameObject.FindGameObjectWithTag("MinBastionHeight").GetComponent<InputField>().text;
        string maxBah = GameObject.FindGameObjectWithTag("MaxBastionHeight").GetComponent<InputField>().text;
        string minBaw = GameObject.FindGameObjectWithTag("MinBastionWidth").GetComponent<InputField>().text;
        string maxBaw = GameObject.FindGameObjectWithTag("MaxBastionWidth").GetComponent<InputField>().text;

        string minTl = GameObject.FindGameObjectWithTag("MinTowerLength").GetComponent<InputField>().text;
        string maxTl = GameObject.FindGameObjectWithTag("MaxTowerLength").GetComponent<InputField>().text;

        // Checking if they're correct
        try
        {
            minCliffHeight = int.Parse(minCh);
            maxCliffHeight = int.Parse(maxCh);
            minCliffWidth = int.Parse(minCw);
            maxCliffWidth = int.Parse(maxCw);

            minBridgeWidth = int.Parse(minBw);
            maxBridgeWidth = int.Parse(maxBw);

            minBastionHeight = int.Parse(minBah);
            maxBastionHeight = int.Parse(maxBah);
            minBastionWidth = int.Parse(minBaw);
            maxBastionWidth = int.Parse(maxBaw);

            minTowerLength = int.Parse(minTl);
            maxTowerLength = int.Parse(maxTl);

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

        startData.bridgePositions.Add(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 6));

        for (int i=0; i<nCliffs; i++)
        {
            if (startData.CanChangeHeight())
            {
                cliffHeight = UnityEngine.Random.Range(
                    (startData.prevHeight == 0) ? 3 : startData.prevHeight, 
                    (startData.prevHeight == 0) ? 3 : maxCliffHeight);
                cliffWidth = UnityEngine.Random.Range(minCliffWidth, maxCliffWidth);
            }
            else
            {
                cliffHeight = startData.prevHeight;
            }
            
            if (i != (nCliffs - 1))
            {
                mustBuildBridge = true;
            }
            else
            {
                mustBuildBridge = false;
            }

            startData = GenerateCliff(startData, cliffHeight, cliffWidth, mustBuildBridge);
            GenerateWall(startData, cliffWidth);
        }
    }

    private void GenerateWall(CliffGenerationData data, int width)
    {
        // Setting the tileset
        currentTileset = wallTileset;
        // Getting start position
        Vector2 startPos = data.wallsPositions[0];
        // Converting it to int
        int startX = (int)startPos.x;
        int startY = (int)startPos.y;
        int endX = startX + width;

        // There's already grass on the cliff
        startPos = new Vector2(startX, startY);

        for (int x = startX; x < endX; x++)
        {
            string[] possibleTiles = GetPossibleTiles(startX, endX, startY, startY + 1, x, startY);
            string toInstantiate = "";

            // Left or right tile
            if ((startX == x) || (x == (endX - 1)))
            {
                toInstantiate = possibleTiles[UnityEngine.Random.Range(0, possibleTiles.Length)];
            }
            else
            {
                // Walls are divided in 2 parts: light and dark tiles
                if ((x - startX) < ((endX - startX) / 2))
                {
                    // Instantiate light tiles
                    toInstantiate = possibleTiles[UnityEngine.Random.Range(2, possibleTiles.Length)];
                }
                else
                {
                    // Instantiate dark tiles
                    toInstantiate = possibleTiles[UnityEngine.Random.Range(0, 2)];
                }
            }

            // Actually instantiating the tile
            GameObject instantiated = Instantiate(
                (GameObject)Resources.Load(toInstantiate),
                new Vector2(x, startY),
                Quaternion.Euler(Vector3.zero)
                );
            instantiated.transform.parent = generationParent.transform;
        }

        // Generating a bastion behind the wall 
        GenerateBastion(data, width, startX, endX);
    }

    private void GenerateBastion(CliffGenerationData data, int maxWidth, int cliffStart, int cliffEnd)
    {
        Vector3 startPos = data.wallsPositions[0];

        int xStart = (int)startPos.x + 1;
        int yStart = (int)startPos.y;

        int width = UnityEngine.Random.Range(minBastionWidth, ((cliffEnd - xStart) > maxBastionWidth) ? maxBastionWidth : (cliffEnd - xStart));
        int height = UnityEngine.Random.Range(minBastionHeight, maxBastionHeight);

        int yEnd = yStart + height;
        int xEnd = xStart + width - 1;

        currentBastionTileset = bastionTilesets[UnityEngine.Random.Range(0, bastionTilesets.Length)];

        Debug.Log("Width: " + width + ", startx: " + xStart + ", endx: " + xEnd);

        for (int x=xStart; x<xEnd; x++)
        {
            Debug.Log("Ci sono");
            for (int y = yStart; y < yEnd; y++)
            {
                string[] possibleTiles = GetPossibleBastionTiles(xStart, xEnd, yStart, yEnd, x, y);
                string toInstantiate = possibleTiles[UnityEngine.Random.Range(0, possibleTiles.Length)];

                GameObject instantiated = Instantiate(
                    (GameObject)Resources.Load(toInstantiate),
                    new Vector3(x, y),
                    Quaternion.Euler(Vector3.zero)
                );
                instantiated.transform.parent = generationParent.transform;
            }
        }
        
        if ((xStart + width) <= (cliffStart + (cliffEnd - cliffStart) / 1.4f) && ((xEnd + 8) != cliffEnd))
        {
            Debug.Log("Ue");
            float newStartX = UnityEngine.Random.Range(data.wallsPositions[0].x + (width - width / 3.5f), data.wallsPositions[0].x + width);
            data.wallsPositions[0] = new Vector3(newStartX, data.wallsPositions[0].y);

            GenerateBastion(data, maxWidth, cliffStart, cliffEnd);
        }
        
    }

    private void GenerateGrass(int xStart, int yEnd, int xEnd)
    {
        for (int x=xStart; x<xEnd; x++)
        {
            for (int y = yEnd - grassHeight; y<yEnd; y++)
            {
                if (y == (-yEnd - 1) && UnityEngine.Random.Range(0, 100) < 50)
                {
                    // TODO: instantiate irregularities
                    GameObject tmp = Instantiate(
                        grassIrregularities,
                        new Vector3(x, y + 1),
                        Quaternion.Euler(Vector3.zero));
                    tmp.transform.parent = generationParent.transform;
                }

                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    bool flipX = UnityEngine.Random.Range(0, 100) < 50 ? true : false;
                    bool flipY = UnityEngine.Random.Range(0, 100) < 50 ? true : false;
                    SpriteRenderer sr;

                    GameObject tmp = Instantiate(
                        flowers,
                        new Vector3(x, y),
                        Quaternion.Euler(Vector3.zero));
                    tmp.transform.parent = generationParent.transform;

                    sr = tmp.GetComponent<SpriteRenderer>();
                    sr.flipX = flipX;
                    sr.flipY = flipY;
                }

                GameObject instantiated = Instantiate(
                    grassMiddle,
                    new Vector3(x, y),
                    Quaternion.Euler(Vector3.zero)
                );
                instantiated.transform.parent = generationParent.transform;
                instantiated.SetActive(false);
                instantiated.SetActive(true);
            }
        }
    }

    private CliffGenerationData GenerateCliff(CliffGenerationData data, int height, int width, bool mustBuildBridge)
    {
        Vector3 start = data.bridgePositions[0];
        CliffGenerationData ret = new CliffGenerationData();

        // Converting start position to int
        int xStart = (int)start.x;
        int yStart = (int)start.y;
        // Crating an int start position
        Vector2 intStartPos = new Vector2(xStart, yStart);

        // Calculating end position
        int xEnd = xStart + width;
        int yEnd = yStart + height;

        GenerateGrass(xStart, yStart, xEnd);

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

                    // Generate grass under the bridge
                    GenerateGrass(bridgeStartX, bridgeStartY, bridgeEndX);

                    // Choosing a random bridge tileset
                    currentTileset = bridgeTilesets[UnityEngine.Random.Range(0, bridgeTilesets.Length)];
                    prefix = currentTileset.prefix;

                    // If I didn't pick a big bridge, I can make it shorter than the cliff
                    if (!((prefix.Contains("/LightBridge/")) || prefix.Contains("/DarkBridge/") || prefix.Contains("Brown")))
                    {
                        bridgeEndY = UnityEngine.Random.Range(bridgeStartY + 3, yEnd);
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

                                /* The bottom right corner of the bridge is the position of the next 
                                 * cliff, so I add it to the bridgePositions list */
                                if (possibleTiles[0].Contains("BottomRight"))
                                {
                                    Vector2 toAdd = new Vector2(instantiationPos.x, instantiationPos.y);
                                    ret.bridgePositions.Add(toAdd);
                                    // Also, since the bridge contains part of the cliff, I don't need to draw that part
                                    ret.mustDrawLeft = false;
                                    // Saving bridge type
                                    ret.bridgeType = currentTileset.prefix;
                                    // Adding current Y
                                    ret.prevHeight = height;
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
                    if (((x == xStart) && data.mustDrawLeft) || (x != xStart) || (data.prevHeight <= height) /*||
                        (data.bridgeType.Contains("Little") && (x == xStart) && (y == (yEnd - 1)))*/)
                    {
                        string[] possibleTiles = GetPossibleTiles(xStart, xEnd, yStart, yEnd, x, y);
                        string tile = possibleTiles[0];

                        if (possibleTiles.Length != 1)
                        {
                            int index = UnityEngine.Random.Range(0, possibleTiles.Length);
                            tile = possibleTiles[index];
                        }

                        // If this is the top right corner, I have to build a wall starting from it
                        if (tile.Contains("TopLeft"))
                        {
                            ret.wallsPositions.Add(new Vector2(x, y));
                        }

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
    private string[] GetPossibleBastionTiles(int xStart, int xEnd, int yStart, int yEnd, int x, int y)
    {
        // Left tiles
        if (x == xStart)
        {
            // Bottom left tile
            if (y == yStart)
            {
                return currentBastionTileset.GetBottomLeftLight();
            }
            // Top left tile
            else if (y == (yEnd - 1))
            {
                return currentBastionTileset.GetTopLeftLight();
            }
            // Middle left tile
            else
            {
                return currentBastionTileset.GetMiddleLeftLight();
            }
        }
        // Right light tiles
        else if (x == (xStart + ((xEnd - xStart) / 2)))
        {
            // Bottom right tile
            if (y == yStart)
            {
                return currentBastionTileset.GetBottomRightLight();
            }
            // Top right tile
            else if (y == (yEnd - 1))
            {
                return currentBastionTileset.GetTopRightLight();
            }
            else
            {
                return currentBastionTileset.GetMiddleRightLight();
            }
        }
        // Middle tiles
        else if (x < (xStart + ((xEnd - xStart) / 2)))
        {
            // Bottom middle tile
            if (y == yStart)
            {
                return currentBastionTileset.GetBottomMiddleLight();
            }
            // Top middle tile
            else if (y == (yEnd - 1))
            {
                return currentBastionTileset.GetTopMiddleLight();
            }
            // Middle middle tile
            else
            {
                return currentBastionTileset.GetMiddleMiddleLight();
            }
        }
        else if (x == (xStart + ((xEnd - xStart) / 2 + 1)))
        {
            // Bottom left tile
            if (y == yStart)
            {
                return currentBastionTileset.GetBottomLeftDark();
            }
            // Top left tile
            else if (y == (yEnd - 1))
            {
                return currentBastionTileset.GetTopLeftDark();
            }
            // Middle left tile
            else
            {
                return currentBastionTileset.GetMiddleLeftDark();
            }
        }
        // Right dark tiles
        else if (x == (xEnd - 1))
        {
            // Bottom right tile
            if (y == yStart)
            {
                return currentBastionTileset.GetBottomRightDark();
            }
            // Top right tile
            else if (y == (yEnd - 1))
            {
                return currentBastionTileset.GetTopRightDark();
            }
            else
            {
                return currentBastionTileset.GetMiddleRightDark();
            }
        }
        // Middle tiles
        else
        {
            // Bottom middle tile
            if (y == yStart)
            {
                return currentBastionTileset.GetBottomMiddleDark();
            }
            // Top middle tile
            else if (y == (yEnd - 1))
            {
                return currentBastionTileset.GetTopMiddleDark();
            }
            // Middle middle tile
            else
            {
                return currentBastionTileset.GetMiddleMiddleDark();
            }
        }
    }

    /** Returns the right bastion tile depending on the rect coordinates and tiles relative position
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
