using UnityEngine;

public interface IColor
{
    public Color GetColor();

    public void SetColor(Color color);

    public float GetAlpha();

    public void SetAlpha(float alpha);

    public void GetColorAndAlpha(out Color color, out float alpha, out bool canGetColor);
    public void SetColorAndAlpha(Color color, float alpha);
}


