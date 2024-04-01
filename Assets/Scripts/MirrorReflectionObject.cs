using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflectionObject : MonoBehaviour
{
    [SerializeField] private ColorType mirrorColorType = ColorType.Transparent;
    public ColorType MirrorColorType { get => mirrorColorType; set => mirrorColorType = value; }
}
