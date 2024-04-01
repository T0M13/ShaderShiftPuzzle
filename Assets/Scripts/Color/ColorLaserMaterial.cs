using UnityEngine;

[CreateAssetMenu(fileName = "ColorLaserMaterial", menuName = "ScriptableObjects/ColorLaserMaterial")]
public class ColorLaserMaterial : ScriptableObject
{
    public Material material;
    public ColorType colorType;
}
