using UnityEngine;
using UnityEngine.Events;

public class ColorSensitiveTrigger : MonoBehaviour, IEnergy
{
    public bool isEnergyOn = false;
    public bool needsToStay = true;

    [SerializeField] private Color targetColor;
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField]
    protected UnityEvent onEnergyReceived;

    [SerializeField]
    protected UnityEvent onEnergyGone;

    private void Start()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.InitializeSound("ElectricLoopedHigh", gameObject);
        }
    }

    private void CheckLaserColor(Color laserColor)
    {
        if (IsColorMatch(laserColor))
        {
            isEnergyOn = true;
            onEnergyReceived?.Invoke();
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

    public void OnEnergy(LaserEnd laserEnd)
    {
        if (!isEnergyOn)
        {
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

    public void OffEnergy(LaserEnd laserEnd)
    {
        if (!needsToStay) return;
        if (isEnergyOn)
        {
            isEnergyOn = false;
            onEnergyGone?.Invoke();
        }
    }

    public void PlaySound(string name)
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySound(name, gameObject);
        }
    }

    public void StopSound()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.StopSound(gameObject);
        }
    }
}
