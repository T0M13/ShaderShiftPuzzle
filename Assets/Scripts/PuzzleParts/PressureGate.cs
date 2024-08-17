using UnityEngine;

public class PressureGate : Gate
{
    [SerializeField] private float pressureRequiredToFullyOpen = 4f;
    [SerializeField][ShowOnly] private float currentPressure = 0f;

    protected override void Start()
    {
        base.Start();
        UpdateGatePositionOnStart(currentPressure);
    }

    public void UpdateGatePositionOnStart(float pressure)
    {
        currentPressure = pressure;
        float openRatio = Mathf.Clamp(currentPressure / pressureRequiredToFullyOpen, 0, 1);
        targetPosition = Vector3.Lerp(closedPosition, openedPosition, openRatio);
        StopAllCoroutines();
        StartCoroutine(MoveGate(targetPosition));
        if (currentPressure >= pressureRequiredToFullyOpen)
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }

    public void UpdateGatePosition(float pressure)
    {
        currentPressure = pressure;
        float openRatio = Mathf.Clamp(currentPressure / pressureRequiredToFullyOpen, 0, 1);
        targetPosition = Vector3.Lerp(closedPosition, openedPosition, openRatio);
        StopAllCoroutines();
        PlayGateSound();
        StartCoroutine(MoveGate(targetPosition));
        if(currentPressure >= pressureRequiredToFullyOpen)
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }
}
