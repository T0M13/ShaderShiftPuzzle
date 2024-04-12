using UnityEngine;

public class ColorSensitiveTrigger : MonoBehaviour
{
    [SerializeField] private Color targetColor;
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private bool actionPerformed = false;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay called with: " + other.gameObject.name);
        if (actionPerformed) return;

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

    private void CheckLaserColor(Color laserColor)
    {
        if (IsColorMatch(laserColor) && !actionPerformed)
        {
            PerformAction();
            actionPerformed = true;
        }
    }

    private bool IsColorMatch(Color color)
    {
        // Using Unity's built-in method to convert colors to strings for precise comparison/logging
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
        Debug.Log("Action performed: Color match!");
    }

    public void ResetAction()
    {
        actionPerformed = false;
    }
}
