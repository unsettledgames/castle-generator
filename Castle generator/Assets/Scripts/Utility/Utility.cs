using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Vector3 GetPixelledPosition(Vector3 tmp)
    {
        float rightX;
        float rightY;
        float ratio = 1f / 32f;

        rightX = tmp.x - (tmp.x % ratio);
        rightY = tmp.y - (tmp.y % ratio);

        // Vector3 clamped_position = new Vector3(((int)theoricalPosition.x) / (float)ratio, ((int)theoricalPosition.y) / (float)ratio, transform.position.z);

        return new Vector3(rightX, rightY, tmp.z);
    }
}
