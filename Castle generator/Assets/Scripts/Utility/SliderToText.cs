using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderToText : MonoBehaviour
{
    public Slider slider;

    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "" + slider.value;
    }
}
