using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour, IColor
{
    protected static string _DEFAULTCOLOR = "_DefaultColor";
    protected static string _DEFAULTALPHA = "_DefaultAlpha";
    protected static string _ALPHA = "_Alpha";
    protected static string _COLOR = "_Color";
    [SerializeField] protected Material myMaterial;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected ColorSettingsComponent colorSettings;
    protected Coroutine resetObjectCor;


    protected virtual void OnValidate()
    {
        GetRefs();
    }

    protected virtual void Awake()
    {
        GetRefs();
        ResetObject();
    }

    protected void GetRefs()
    {
        myMaterial = GetComponent<Renderer>().sharedMaterial;

        if (myCollider == null)
            myCollider = GetComponent<Collider>();
    }

    protected void ResetObject()
    {
        SetColorAndAlpha(myMaterial.GetColor(_DEFAULTCOLOR), myMaterial.GetFloat(_DEFAULTALPHA));
    }

    public virtual Color GetColor()
    {
        return myMaterial.GetColor(_COLOR);
    }

    public virtual float GetAlpha()
    {
        return myMaterial.GetFloat(_ALPHA);
    }

    public virtual void GetColorAndAlpha(out Color color, out float alpha, out bool canGetColor)
    {
        canGetColor = colorSettings.canGetColor;
        InteractionBeforeGet();
        color = GetColor();
        alpha = GetAlpha();
        InteractionAfterGet();
    }

    public virtual void SetColor(Color color)
    {
        myMaterial.SetColor(_COLOR, color);
    }

    public virtual void SetAlpha(float alpha)
    {
        myMaterial.SetFloat(_ALPHA, alpha);
    }

    public virtual void SetColorAndAlpha(Color color, float alpha)
    {
        if (!colorSettings.canSetColor) return;
        InteractionBeforeSet();
        SetColor(color);
        SetAlpha(alpha);
        InteractionAfterSet();
    }

    public virtual void SetSelfColorAndAlpha(Color color, float alpha)
    {
        InteractionBeforeSet();
        SetColor(color);
        SetAlpha(alpha);
        InteractionAfterSet();
    }

    protected virtual void InteractionAfterSet()
    {
        if (colorSettings.canBecomeInvisible)
        {
            InvisibleInteraction();
        }

        if (colorSettings.canReset)
        {
            ResetInteraction();
        }
    }

    protected virtual void InteractionAfterGet()
    {

    }

    protected virtual void InteractionBeforeSet()
    {

    }

    protected virtual void InteractionBeforeGet()
    {

    }


    #region Interactions

    private void InvisibleInteraction()
    {
        if (GetAlpha() <= colorSettings.invisiblityThreshold)
        {
            myCollider.enabled = false;
        }
    }

    private void ResetInteraction()
    {
        if (colorSettings.canReset)
        {
            if (resetObjectCor != null)
                StopCoroutine(resetObjectCor);
            resetObjectCor = StartCoroutine(IEInvisibleInteraction());
        }
    }

    private IEnumerator IEInvisibleInteraction()
    {
        yield return new WaitForSeconds(colorSettings.timeTillReset);
        ResetObject();
        myCollider.enabled = true;
    }

    #endregion

}
