using UnityEngine;

public static class ColorUtils
{
   public static Color MixColors(Color colorA, Color colorB)
    {
        return Color.Lerp(colorA, colorB, 0.5f);
    }

    public static float MixAlphas(float alphaA, float alphaB)
    {
        float alpha = Mathf.Lerp(alphaA, alphaB, 0.5f);

        return alpha;
    }
}
