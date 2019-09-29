using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public Gradient lightColours;
    public float daySpeed;
    public Slider daySpeedCycle;

    private Light2D light;
    private float currentIndex;
    

    private void Start()
    {
        currentIndex = 0;
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        light.color = lightColours.Evaluate(currentIndex);
        currentIndex += Time.deltaTime * daySpeed;

        if (currentIndex > 1)
        {
            currentIndex = 0;
        }
    }

    public void SetDaySpeed()
    {
        daySpeed = daySpeedCycle.value;
    }
}
