using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public Transform[] transformsToActivateOnPause;
    public Transform[] transformsToDeactivateOnPause;
    public Action<bool> ToggleCanvasPause;

    void Start()
    {
        if (canvasScaler == null)
            canvasScaler = gameObject.GetComponent<CanvasScaler>();
    }

    public void ScaleCanvas(int scale = 1080)
    {
        canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.x, scale);
    }

}
