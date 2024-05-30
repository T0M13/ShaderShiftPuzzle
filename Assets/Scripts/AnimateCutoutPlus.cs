using AmazingAssets.AdvancedDissolve;
using System.Collections;
using UnityEngine;

public class AnimateCutoutPlus : MonoBehaviour
{
    public Material material;
    [SerializeField][ShowOnly] private float clipValue = 0;
    [SerializeField] private float dissolveSpeed = 1f;
    [SerializeField] private bool activated = true;

    private Coroutine dissolveCoroutine;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        SetInitialClipValue();
    }

    private void SetInitialClipValue()
    {
        if (activated)
        {
            clipValue = 0;
        }
        else
        {
            clipValue = 1;

        }

        UpdateMaterialClipValue();
    }

    public void StartDissolve(bool activate)
    {
        if (dissolveCoroutine != null)
            StopCoroutine(dissolveCoroutine);

        if (activate)
        {
            clipValue = 1;
        }
        else
        {
            clipValue = 0;

        }

        dissolveCoroutine = StartCoroutine(animateDissolve(activate));
    }

    private IEnumerator animateDissolve(bool activate)
    {
        clipValue = activate ? 0 : 1;
        float step = activate ? dissolveSpeed : -dissolveSpeed;

        while ((activate && clipValue < 1) || (!activate && clipValue > 0))
        {
            clipValue += Time.deltaTime * step;
            clipValue = Mathf.Clamp(clipValue, 0, 1);
            UpdateMaterialClipValue();
            yield return null;
        }

        activated = !activate;
    }

    private void UpdateMaterialClipValue()
    {
        AmazingAssets.AdvancedDissolve.AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clipValue);
    }
}
