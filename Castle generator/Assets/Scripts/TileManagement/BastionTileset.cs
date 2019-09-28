using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BastionTileset : MonoBehaviour
{
    public string prefix = "";

    [Header("Light tiles")]
    public string[] topLeftLight;
    public string[] topMiddleLight;
    public string[] topRightLight;
    public string[] middleRightLight;
    public string[] bottomRightLight;
    public string[] bottomMiddleLight;
    public string[] bottomLeftLight;
    public string[] middleLeftLight;
    public string[] middleMiddleLight;

    [Header("Dark tiles")]
    public string[] topLeftDark;
    public string[] topMiddleDark;
    public string[] topRightDark;
    public string[] middleRightDark;
    public string[] bottomRightDark;
    public string[] bottomMiddleDark;
    public string[] bottomLeftDark;
    public string[] middleLeftDark;
    public string[] middleMiddleDark;

    public string[] GetTopLeftLight()
    {
        return AddPrefix(topLeftLight);
    }
    public string[] GetTopMiddleLight()
    {
        return AddPrefix(topMiddleLight);
    }
    public string[] GetTopRightLight()
    {
        return AddPrefix(topRightLight);
    }
    public string[] GetMiddleRightLight()
    {
        return AddPrefix(middleRightLight);
    }
    public string[] GetBottomRightLight()
    {
        return AddPrefix(bottomRightLight);
    }
    public string[] GetBottomMiddleLight()
    {
        return AddPrefix(bottomMiddleLight);
    }
    public string[] GetBottomLeftLight()
    {
        return AddPrefix(bottomLeftLight);
    }
    public string[] GetMiddleLeftLight()
    {
        return AddPrefix(middleLeftLight);
    }
    public string[] GetMiddleMiddleLight()
    {
        return AddPrefix(middleMiddleLight);
    }

    public string[] GetTopLeftDark()
    {
        return AddPrefix(topLeftDark);
    }
    public string[] GetTopMiddleDark()
    {
        return AddPrefix(topMiddleDark);
    }
    public string[] GetTopRightDark()
    {
        return AddPrefix(topRightDark);
    }
    public string[] GetMiddleRightDark()
    {
        return AddPrefix(middleRightDark);
    }
    public string[] GetBottomRightDark()
    {
        return AddPrefix(bottomRightDark);
    }
    public string[] GetBottomMiddleDark()
    {
        return AddPrefix(bottomMiddleDark);
    }
    public string[] GetBottomLeftDark()
    {
        return AddPrefix(bottomLeftDark);
    }
    public string[] GetMiddleLeftDark()
    {
        return AddPrefix(middleLeftDark);
    }
    public string[] GetMiddleMiddleDark()
    {
        return AddPrefix(middleMiddleDark);
    }


    private string[] AddPrefix(string[] tileNames)
    {
        string[] ret = new string[tileNames.Length];

        for (int i = 0; i < tileNames.Length; i++)
        {
            ret[i] = prefix + tileNames[i];
        }

        return ret;
    }
}
