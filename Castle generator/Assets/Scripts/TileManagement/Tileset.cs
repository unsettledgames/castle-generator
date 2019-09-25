using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileset : MonoBehaviour
{
    public string prefix = "";

    public string[] topLeft;
    public string[] topMiddle;
    public string[] topRight;
    public string[] middleRight;
    public string[] bottomRight;
    public string[] bottomMiddle;
    public string[] bottomLeft;
    public string[] middleLeft;
    public string[] middleMiddle;

    public string[] GetTopLeft()
    {
        return AddPrefix(topLeft);
    }
    public string[] GetTopMiddle()
    {
        return AddPrefix(topMiddle);
    }
    public string[] GetTopRight()
    {
        return AddPrefix(topRight);
    }
    public string[] GetMiddleRight()
    {
        return AddPrefix(middleRight);
    }
    public string[] GetBottomRight()
    {
        return AddPrefix(bottomRight);
    }
    public string[] GetBottomMiddle()
    {
        return AddPrefix(bottomMiddle);
    }
    public string[] GetBottomLeft()
    {
        return AddPrefix(bottomLeft);
    }
    public string[] GetMiddleLeft()
    {
        return AddPrefix(middleLeft);
    }
    public string[] GetMiddleMiddle()
    {
        return AddPrefix(middleMiddle);
    }


    private string[] AddPrefix(string[] tileNames)
    {
        string[] ret = new string[tileNames.Length];

        for (int i=0; i<tileNames.Length; i++)
        {
            ret[i] = prefix + tileNames[i];
        }

        return ret;
    }
}
