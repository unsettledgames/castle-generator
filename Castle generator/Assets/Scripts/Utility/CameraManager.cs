using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // The higher it is, the less smooth the movement is
    public float smoothFactor;
    // The higher it is, the faster the movement is
    public float speed;
    // Camera boundaries
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Update is called once per frame
    void Update()
    {
        // Input data
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        // Calculating the destination position
        float destinationPosX = transform.position.x + (xInput * smoothFactor * Time.deltaTime) * speed;
        float destinationPosY = transform.position.y + (yInput * smoothFactor * Time.deltaTime) * speed;

        // Setting new position, but adapting it to the pixel grid (so that the rendering is still pixel perfect
        transform.position = Utility.GetPixelledPosition(new Vector3(destinationPosX, destinationPosY, Consts.cameraZPosition));
    }
}
