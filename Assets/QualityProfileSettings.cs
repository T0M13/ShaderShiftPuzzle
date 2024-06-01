using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Change this if you use a different render pipeline

public class QualityProfileSettings : MonoBehaviour
{
    private static QualityProfileSettings instance = null;

    [SerializeField] private Volume globalVolume;

    [SerializeField] private LiftGammaGain liftGammaGain;
    [SerializeField] private ColorAdjustments colorAdjustments;

    [SerializeField] private float defaultGamma = 0;
    [SerializeField] private float defaultBrightness = 0;

    public static QualityProfileSettings Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Ensure the Volume component has the required overrides
        if (!globalVolume.profile.TryGet<LiftGammaGain>(out liftGammaGain))
        {
            Debug.LogWarning("LiftGammaGain not found on global volume profile.");
        }

        if (!globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            Debug.LogWarning("ColorAdjustments not found on global volume profile.");
        }

        SetPostExposure(defaultBrightness);
        SetGamma(defaultGamma);
    }

    private void OnValidate()
    {
        if (!globalVolume.profile.TryGet<LiftGammaGain>(out liftGammaGain))
        {
            Debug.LogWarning("LiftGammaGain not found on global volume profile.");
        }

        if (!globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            Debug.LogWarning("ColorAdjustments not found on global volume profile.");
        }
    }

    public void SetGamma(float gamma)
    {
        if (liftGammaGain != null)
        {
            liftGammaGain.gamma.value = new Vector4(gamma, gamma, gamma, gamma);
            liftGammaGain.gamma.overrideState = true;
        }

    }

    public void SetPostExposure(float exposure)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = exposure;
            colorAdjustments.postExposure.overrideState = true;

        }

    }
}
