using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflectionObject : MonoBehaviour
{
    [SerializeField] private Color mirrorColor;
    public Color MirrorColor { get => mirrorColor; set => mirrorColor = value; }
}
