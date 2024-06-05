using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMirrorObject : ColorObject
{

    [SerializeField] private MirrorReflectionObject mirror;
    private MaterialPropertyBlock propBlock;
    private Renderer myRenderer;
    protected override void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        GetRefs();
        ResetObject();
    }

    protected override void GetRefs()
    {
        myMaterial = GetComponent<Renderer>().sharedMaterial;

        myRenderer = GetComponent<Renderer>();

        if (myCollider == null)
            myCollider = GetComponent<Collider>();

        if (mirror != null)
        {
           myRenderer.sharedMaterial.SetColor(_DEFAULTCOLOR, mirror.MirrorColor);
        }
    }

    public override void SetColorAndAlpha(Color color, float alpha)
    {
        InteractionBeforeSet();
        SetColor(color);
        SetAlpha(alpha);
        if (mirror != null)
        {
            mirror.MirrorColor = color;
        }
        InteractionAfterSet();
    }

    protected override void ResetObject()
    {
        Color defaultColor = myRenderer.sharedMaterial.GetColor(_DEFAULTCOLOR);
        float defaultAlpha = myRenderer.sharedMaterial.GetFloat(_DEFAULTALPHA);
        SetColorAndAlpha(defaultColor, defaultAlpha);
    }

    public override void SetColor(Color color)
    {
        myRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(_COLOR, color);
        myRenderer.SetPropertyBlock(propBlock);
    }

    public override void SetAlpha(float alpha)
    {
        myRenderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat(_ALPHA, alpha);
        myRenderer.SetPropertyBlock(propBlock);
    }

}
