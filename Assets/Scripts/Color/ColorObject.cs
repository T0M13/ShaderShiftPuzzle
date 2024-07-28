using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorObject : MonoBehaviour, IColor
{
    protected static string _DEFAULTCOLOR = "_DefaultColor";
    protected static string _DEFAULTALPHA = "_DefaultAlpha";
    protected static string _ALPHA = "_Alpha";
    protected static string _COLOR = "_Color";
    [Header("Current Settings")]
    [SerializeField] private Color currentColor;
    [SerializeField] protected float currentAlpha = 1;
    [Header("Default Settings")]
    [SerializeField] protected Color defaultColor;
    [SerializeField] protected float defaultAlpha = 1;
    [SerializeField] protected MaterialPropertyBlock propBlock;
    [SerializeField] protected Renderer myRenderer;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected ColorSettingsComponent colorSettings;
    protected Coroutine resetObjectCor;
    [Header("Events")]

    public UnityEvent onBeforeSet;
    public UnityEvent onBeforeGet;
    public UnityEvent onAfterSet;
    public UnityEvent onAfterGet;

    public Color CurrentColor { get => currentColor; set => currentColor = value; }

    protected virtual void Awake()
    {
        GetRefs();
        ResetObject();
    }

    protected virtual void GetRefs()
    {
        if (myCollider == null)
            myCollider = GetComponent<Collider>();
        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();
    }

    protected virtual void ResetObject()
    {
        myRenderer.material = new Material(myRenderer.sharedMaterial);
        SetColor(defaultColor);
        SetAlpha(defaultAlpha);
    }

    public virtual Color GetColor()
    {
        return myRenderer.material.GetColor(_COLOR);
    }

    public virtual float GetAlpha()
    {
        return myRenderer.material.GetFloat(_ALPHA);
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
        CurrentColor = color;
        myRenderer.material.SetColor(_COLOR, color);
    }

    public virtual void SetAlpha(float alpha)
    {
        currentAlpha = alpha;
        myRenderer.material.SetFloat(_ALPHA, alpha);
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
        onAfterSet?.Invoke();
    }

    protected virtual void InteractionAfterGet()
    {
        onAfterGet?.Invoke();
    }

    protected virtual void InteractionBeforeSet()
    {
        onBeforeSet?.Invoke();
    }

    protected virtual void InteractionBeforeGet()
    {
        onBeforeGet?.Invoke();
    }

    #region Interactions

    public void InvisibleInteraction()
    {
        if (GetAlpha() <= colorSettings.invisiblityThreshold)
        {
            myCollider.enabled = false;
        }
    }

    public void ResetInteraction()
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
