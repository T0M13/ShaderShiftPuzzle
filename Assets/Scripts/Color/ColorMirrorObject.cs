using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMirrorObject : ColorObject
{
    [SerializeField] private MirrorReflectionObject mirrorObjectRef;

    public override void SetColor(Color color)
    {
        base.SetColor(color);

        if(mirrorObjectRef != null)
        {
            mirrorObjectRef.ChangeColor(color);
        }
    }
}
