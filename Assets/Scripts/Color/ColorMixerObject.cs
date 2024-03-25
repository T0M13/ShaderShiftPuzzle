using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMixerObject : ColorObject
{
    public override void SetColorAndAlpha(Color color, float alpha)
    {
        InteractionBeforeSet();
        SetColor(MixColors(GetColor(), color));
        SetAlpha(MixAlphas(GetAlpha(), alpha));
        InteractionAfterSet();
    }

    public Color MixColors(Color mixingColor, Color colorToAdd)
    {
        Color mixedColor = new Color(
            Mathf.Lerp(mixingColor.r, colorToAdd.r, 0.5f),
        Mathf.Lerp(mixingColor.g, colorToAdd.g, 0.5f),
        Mathf.Lerp(mixingColor.b, colorToAdd.b, 0.5f),
        Mathf.Lerp(mixingColor.a, colorToAdd.a, 0.5f)
        );

        return mixedColor;
    }

    public float MixAlphas(float mixingAlpha, float alphaToAdd)
    {
        float alpha = Mathf.Lerp(mixingAlpha, alphaToAdd, 0.5f);

        return alpha;
    }




}
