using UnityEngine;

public class ColorSensitiveTrigger : EnergyPortPuzzle
{
    [SerializeField] private Color targetColor;
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private bool performOnce = true;
    [SerializeField][ShowOnly] private bool performed = false;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay called with: " + other.gameObject.name);
        if (performed) return;

        if (other.GetComponent<LaserEnd>())
        {
            var laserEnd = other.GetComponent<LaserEnd>();

            if (laserEnd.LaserParent)
            {
                CheckLaserColor(laserEnd.LaserParent.CurrentColor);
            }

            if (laserEnd.LaserCloneParent)
            {
                CheckLaserColor(laserEnd.LaserCloneParent.CurrentColor);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LaserEnd>())
        {
            ResetAction();
        }
    }

    private void CheckLaserColor(Color laserColor)
    {
        if (IsColorMatch(laserColor))
        {
            PerformAction();
        }
    }

    private bool IsColorMatch(Color color)
    {
        string targetColorString = ColorUtility.ToHtmlStringRGB(targetColor);
        string incomingColorString = ColorUtility.ToHtmlStringRGB(color);

        // Logging the color strings for debugging
        Debug.Log($"Comparing target color {targetColorString} with incoming color {incomingColorString}");

        // Calculate Euclidean distance in RGB space
        float distance = Mathf.Sqrt(
            Mathf.Pow(color.r - targetColor.r, 2) +
            Mathf.Pow(color.g - targetColor.g, 2) +
            Mathf.Pow(color.b - targetColor.b, 2));

        Debug.Log($"Distance between colors: {distance}");

        return distance <= tolerance;
    }

    private void PerformAction()
    {
        if (performOnce && !performed)
        {
            performed = true;
            OnEnergy();
        }
    }

    public void ResetAction()
    {
        if (performOnce) return;
        OffEnergy();
    }
}
