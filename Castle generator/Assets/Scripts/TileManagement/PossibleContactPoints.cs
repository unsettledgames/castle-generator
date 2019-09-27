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


    public CliffGenerationData()
    {
        // Initializing contact points
        bridgePositions = new List<Vector3>();
        bastionPositions = new List<Vector3>();
    }
}
