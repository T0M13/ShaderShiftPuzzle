using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorSettingsComponent", menuName = "Color Behaviours/ColorSettingsBehaviour")]
public class ColorSettingsComponent : ScriptableObject, ColorSettingsBehaviour
{
    [Header("Get Color")]
    public bool canGetColor;
    [Header("Set Color")]
    public bool canSetColor;
    [Header("Invisibility")]
    public bool canBecomeInvisible;
    public float invisiblityThreshold = 0.2f;
    [Header("Resetable")]
    public bool canReset = false;
    public float timeTillReset = 5f;
}
