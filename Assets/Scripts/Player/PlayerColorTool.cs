using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColorTool : MonoBehaviour
{

    [Header("References")]
    [ShowOnly][SerializeField] private PlayerReferences playerReferences;
    [ShowOnly][SerializeField] private ColorToolManager colorToolManager;
    [SerializeField] public Material colorPickingMaterial;
    [ShowOnly][SerializeField] public Color currentColor;
    [ShowOnly][SerializeField] public float currentAlpha;

    [Header("Settings")]
    [SerializeField] private bool canShoot = true;
    [ShowOnly][SerializeField] private float shot;
    [ShowOnly][SerializeField] private bool isShooting;
    [ShowOnly][SerializeField] private Ray aimPosition;
    [SerializeField] private bool canAim = true;
    [ShowOnly][SerializeField] private float aim;
    [ShowOnly][SerializeField] private bool isAiming;
    [ShowOnly][SerializeField] private Coroutine aimCoroutine;
    [ShowOnly][SerializeField] private Coroutine shootCoroutine;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance = 6;
    [SerializeField] private bool resetColorAfterSetting = true;


    public ColorToolManager ColorToolManager { get => colorToolManager; set => colorToolManager = value; }

    private void Awake()
    {
        GetReferences();

        currentColor = Shader.GetGlobalColor("_PickedColor");
        currentAlpha = Shader.GetGlobalFloat("_PickedAlpha");
    }
    private void GetReferences()
    {
        if (playerReferences == null)
            playerReferences = GetComponent<PlayerReferences>();
    }

    private void Update()
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;

        GetColor();
        SetColor();
    }

    private void GetColor()
    {
        if (!canAim) return;
        if (aim == 0) return;
        if (isAiming) return;

        isAiming = true;

        aimCoroutine = StartCoroutine(IGetColor());
    }

    private IEnumerator IGetColor()
    {
        yield return new WaitForEndOfFrame();

        aimPosition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitData;
        if (Physics.Raycast(aimPosition, out hitData, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hitData.transform.GetComponent<IColor>() != null)
            {
                Color color; float alpha; bool cangetcolor;
                GetColorAndAlpha(hitData, out color, out alpha, out cangetcolor);
                if (cangetcolor)
                {
                    currentColor = color;
                    currentAlpha = alpha;
                    Shader.SetGlobalColor("_PickedColor", currentColor);
                    Shader.SetGlobalFloat("_PickedAlpha", currentAlpha);
                    if (ColorToolManager)
                        ColorToolManager.ChangeOrbColor(color);
                }
            }


        }

        yield return new WaitForSeconds(1f);

        aim = 0;
        isAiming = false;
    }

    void GetColorAndAlpha(RaycastHit hit, out Color color, out float alpha, out bool cangetcolor)
    {
        if (hit.transform.GetComponent<IColor>() != null)
        {
            hit.transform.GetComponent<IColor>().GetColorAndAlpha(out Color icolor, out float ialpha, out bool icangetcolor);
            color = icolor;
            alpha = ialpha;
            cangetcolor = icangetcolor;
        }
        else
        {
            color = Color.black;
            alpha = 1;
            cangetcolor = true;
        }
    }

    void SetColorAt(RaycastHit hit, Color color, float alpha)
    {
        if (hit.transform.GetComponent<IColor>() != null)
        {
            hit.transform.GetComponent<IColor>().SetColorAndAlpha(color, alpha);
        }

        return;

    }

    private void SetColor()
    {
        if (!canShoot) return;
        if (shot == 0) return;
        if (isShooting) return;

        if (ColorToolManager)
            if (ColorToolManager.IsDefaultColor) return;

        isShooting = true;

        shootCoroutine = StartCoroutine(ISetColor());
    }

    private IEnumerator ISetColor()
    {
        yield return new WaitForEndOfFrame();

        aimPosition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitData;
        if (Physics.Raycast(aimPosition, out hitData, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {

            if (colorToolManager)
                colorToolManager.PlayPaintColorAtPosition(currentColor, hitData.point);

            if (hitData.transform.GetComponent<IColor>() != null)
            {
                SetColorAt(hitData, currentColor, currentAlpha);

                if (colorToolManager && resetColorAfterSetting)
                    colorToolManager.ChangeOrbBackToDefaultColor();

            }

        }

        yield return new WaitForSeconds(1f);

        shot = 0;
        isShooting = false;
    }



    Color GetColorAt(RaycastHit hit)
    {
        if (hit.transform.GetComponent<IColor>() != null)
        {
            return hit.transform.GetComponent<IColor>().GetColor();
        }
        else
        {
            return Color.black;
        }
    }

    float GetAlphaAt(RaycastHit hit)
    {
        if (hit.transform.GetComponent<IColor>() != null)
        {
            return hit.transform.GetComponent<IColor>().GetAlpha();
        }
        else
        {
            return 1;
        }
    }

    private bool ActiveToolState()
    {
        if (playerReferences != null)
        {
            if (playerReferences.CurrentToolState == ToolState.ColorTool)
            {
                return true;
            }
        }
        else
        {
            Debug.Log("Reference Missing");
        }
        return false;
    }

    public void OnFire(InputAction.CallbackContext value)
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;
        this.shot = value.ReadValue<float>();
    }

    public void OnAim(InputAction.CallbackContext value)
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;
        this.aim = value.ReadValue<float>();
    }

}
