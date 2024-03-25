using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcherObject : ColorObject
{

    [SerializeField] private float timerDuration = 3f;
    [SerializeField] private float timer;
    [SerializeField] private bool isTimerActive = true;
    [SerializeField] private float timeScale = 1.0f;
    [SerializeField] private List<ColorList> colorList = new List<ColorList>();
    [SerializeField] private int colorIndex = 0;

    private void Update()
    {
        if (colorList.Count <= 0) return;

        if (isTimerActive)
        {
            timer += Time.deltaTime * timeScale;

            if (timer >= timerDuration)
            {
                NextColor();
                timer = 0f;
            }
        }
    }

    private void NextColor()
    {
        if (colorList.Count <= 0) return;
        SetSelfColorAndAlpha(colorList[colorIndex].color, colorList[colorIndex].alpha);
        colorIndex++;
        if (colorIndex >= colorList.Count)
        {
            colorIndex = 0;
        }
    }


    [System.Serializable]
    public class ColorList
    {
        public Color color;
        public float alpha;
    }
}

