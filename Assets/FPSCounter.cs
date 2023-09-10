using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    float lastFrame =0;

    // Update is called once per frame
    void Update()
    {
        lastFrame += (Time.deltaTime - lastFrame) * 0.1f;
        float fps = 1.0f / lastFrame;
        text.text = fps.ToString("00.0");
    }
}
