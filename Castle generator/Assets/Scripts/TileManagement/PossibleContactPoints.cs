using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffGenerationData
{
    // Possible positions to which it is possible to attach bridges
    public List<Vector3> bridgePositions { get; set; }
    // Possible positions to which it is possible to attach bastions
    public List<Vector3> bastionPositions { get; set; }
    // Tells whether the right side must be drawn or not
    public bool mustDrawRight;
    // Tells whether the left side must be drawn or not
    public bool mustDrawLeft;
    // Bridge type (some allow different cliff heights, other don't)
    public string bridgeType;
    // Previous y (used to compensate different cliff heights)
    public int prevHeight;


    public CliffGenerationData()
    {
        // Initializing contact points
        bridgePositions = new List<Vector3>();
        bastionPositions = new List<Vector3>();

        // Setting draw mode
        mustDrawRight = true;
        mustDrawLeft = true;

        // No bridge at the beginning
        bridgeType = "None";
        prevHeight = 0;
    }

    public bool CanChangeHeight()
    {
        if (bridgeType.Contains("Brown") ||
            bridgeType.Contains("/LightBridge") ||
            bridgeType.Contains("/DarkBridge"))
        {
            return false;
        }

        return true;
    }
}
