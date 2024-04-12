using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMixerObject : ColorObject
{
    public override void SetColorAndAlpha(Color color, float alpha)
    {
        InteractionBeforeSet();
        SetColor(ColorUtils.MixColors(GetColor(), color));
        SetAlpha(ColorUtils.MixAlphas(GetAlpha(), alpha));
        InteractionAfterSet();
    }

}
