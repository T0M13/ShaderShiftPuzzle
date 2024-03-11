using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerPostProcessingHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Volume globalVolume;
    [Header("Profile Settings")]
    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private LensDistortion lensDis;
    [Header("Zoom Settings")]
    [SerializeField] private float normalLenDisScale = 1f;
    [SerializeField] private float zoomedLenDisScale = 2.3f;
    [SerializeField] private bool zoomed = false;


    private void OnValidate()
    {
        GetReferences();
    }

    private void Awake()
    {
        GetReferences();
    }


    private void GetReferences()
    {
        if (playerReferences == null)
            playerReferences = GetComponent<PlayerReferences>();

        if (globalVolume == null)
        {
            if (globalVolume = FindObjectOfType<Volume>())
            {
                globalVolume = FindObjectOfType<Volume>();
            }
            else
            {
                Debug.LogWarning("GlobalVolume missing");
            }
        }

        if (volumeProfile == null)
            volumeProfile = globalVolume.profile;

        if (lensDis == null)
            volumeProfile.TryGet(out lensDis);

    }

    private void OnZoom(InputValue value)
    {
        if (globalVolume == null) return;

        if (value.isPressed)
        {
            ToggleZoom();
        }

    }

    private void ToggleZoom()
    {
        if (zoomed)
            lensDis.scale.Override(normalLenDisScale);
        else
            lensDis.scale.Override(zoomedLenDisScale);

        zoomed = !zoomed;

    }



}
