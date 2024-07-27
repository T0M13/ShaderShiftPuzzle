using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static float tickTime = .2f;
    private float tickerTimer;

    public delegate void TickAction();
    public static event TickAction OnTickAction;

    private void Update()
    {
        tickerTimer += Time.deltaTime;
        if(tickerTimer >= tickTime)
        {
            tickerTimer = 0;
            TickEvent();
        }

    }

    private void TickEvent()
    {
        OnTickAction?.Invoke();
    }
}
