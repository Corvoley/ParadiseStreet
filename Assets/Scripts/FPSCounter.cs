using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }

    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        text.text = "Fps: " + Mathf.RoundToInt(CalculateFPS()).ToString();


        //int fps = (int)(1f / Time.deltaTime);
        //text.text = fps.ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;

    }




}
