using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InteractableObject _interactableObject;
    [SerializeField] private HoldableObject _holdableObject;
    [SerializeField] private ColorObject _colorObject;
    [SerializeField] private MeshRenderer _meshR;
    [SerializeField] private Collider _collider;
    [SerializeField] private Color targetColor;
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private bool canBeEaten = false;


    public InteractableObject InteractableObject { get => _interactableObject; set => _interactableObject = value; }
    public HoldableObject HoldableObject { get => _holdableObject; set => _holdableObject = value; }
    public MeshRenderer MeshR { get => _meshR; set => _meshR = value; }
    public Collider Collider { get => _collider; set => _collider = value; }
    public bool CanBeEaten { get => canBeEaten; set => canBeEaten = value; }
    public ColorObject ColorObject { get => _colorObject; set => _colorObject = value; }

    private void Awake()
    {
        if (InteractableObject == null)
            InteractableObject = GetComponent<InteractableObject>();
        if (HoldableObject == null)
            HoldableObject = GetComponent<HoldableObject>();
        if (MeshR == null)
            MeshR = GetComponent<MeshRenderer>();
        if (Collider == null)
            Collider = GetComponent<Collider>();

        if (ColorObject == null)
            ColorObject = GetComponent<ColorObject>();
    }

    public void CheckColor()
    {
        if (IsColorMatch(ColorObject.CurrentColor))
        {
            canBeEaten = true;
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



}
