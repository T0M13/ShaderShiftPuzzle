using Knife.Portal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldTool : MonoBehaviour
{

    [Header("References")]
    [ShowOnly][SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Transform holdArea;
    [Header("Pick Up Settings")]
    //[SerializeField] private float pickUpRange = 15f;
    [SerializeField] private float pickUpForce = 150f;
    [SerializeField] private float moveDirectionThreshold = 0.1f;
    [ShowOnly][SerializeField] private GameObject currentObject;
    [ShowOnly][SerializeField] private HoldableObject currentHoldObject;
    [ShowOnly][SerializeField] private Rigidbody currentObjectBody;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance = 2;
    [SerializeField] private float holdRange = 1.3f;
    [ShowOnly][SerializeField] private bool holdingObject = false;
    [Header("Throw Settings")]
    [SerializeField] private float minThrowForce = 10f;
    [SerializeField] private float maxThrowForce = 40f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float throwThreshold = .5f;
    [ShowOnly][SerializeField] private float holdTimer = 0f;
    [ShowOnly][SerializeField] private bool isThrowing;
    [Header("Object Settings")]
    //[SerializeField] private string ignorePlayerLayer = "IgnorePlayer";
    [ShowOnly][SerializeField] private int currentObjectLayerTemp;

    [Header("Portal Settings")]
    [ShowOnly][SerializeField] private PortalTransientObject currentTransientObjectPortal;
    [ShowOnly][SerializeField] private bool currentTransientTempCanUse;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [ShowOnly][SerializeField] private bool isRotating = false;
    [ShowOnly][SerializeField] private Vector2 rotationPos;

    public bool HoldingObject { get => holdingObject; set => holdingObject = value; }

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        if (playerReferences != null)
            playerReferences = GetComponent<PlayerReferences>();
    }

    private void Update()
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;

        if (currentObject != null)
            if (Vector3.Distance(currentObject.transform.position, holdArea.position) > holdRange)
            {
                DropObject();
            }

        if (isThrowing)
        {
            holdTimer = Mathf.Min(holdTimer + Time.deltaTime, maxChargeTime);

            if (InteractiveMessageUI.Instance)
                InteractiveMessageUI.Instance.SetChargeValue(holdTimer / maxChargeTime);
            else
                Debug.Log("InteractiveMessageUI is missing");
        }

        if (isRotating)
        {
            RotateObject();
        }
    }

    private void FixedUpdate()
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;

        if (currentObject != null)
        {
            MoveObject();
        }
    }

    private void UseHoldTool()
    {
        if (currentObject == null)
        {
            Ray rayposition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit rayhit;
            if (Physics.Raycast(rayposition, out rayhit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                PickUpObject(rayhit.transform.gameObject);
            }
        }
        else
        {
            DropObject();
        }
    }

    private void PickUpObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<IHoldable>() == null) return;

        if (gameObject.GetComponent<HoldableObject>())
        {
            currentHoldObject = gameObject.GetComponent<HoldableObject>();
        }

        if (!currentHoldObject.canBeHeld)
        {
            currentHoldObject = null;
            return;
        }
        else
        {
            currentHoldObject.playerHoldToolRef = this;
        }


        if (gameObject.GetComponent<Rigidbody>())
        {
            currentObjectBody = gameObject.GetComponent<Rigidbody>();
            currentObjectBody.useGravity = false;
            currentObjectBody.drag = 10;
            currentObjectBody.constraints = RigidbodyConstraints.FreezeRotation;

            currentObjectBody.transform.SetParent(holdArea);

            //currentObjectLayerTemp = currentObject.layer;
            //currentObject.layer = LayerMask.NameToLayer(ignorePlayerLayer);
            currentObject = gameObject;


            HoldingObject = true;

        }
        if (gameObject.GetComponent<IPortalTransient>() != null)
        {
            currentTransientObjectPortal = gameObject.GetComponent<PortalTransientObject>();
            currentTransientTempCanUse = currentTransientObjectPortal.CanUsePortal;
            currentTransientObjectPortal.CanUsePortal = false;
        }
    }


    public void DropObject()
    {

        currentHoldObject.playerHoldToolRef = null;
        currentHoldObject = null;

        currentObjectBody.useGravity = true;
        currentObjectBody.drag = 1;
        currentObjectBody.constraints = RigidbodyConstraints.None;

        currentObjectBody.transform.SetParent(null);
        currentObjectBody = null;

        //currentObject.layer = currentObjectLayerTemp;
        //currentObjectLayerTemp = 0;
        currentObject = null;
        HoldingObject = false;

        isThrowing = false;
        holdTimer = 0f;

        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ResetChargeSlider();
        else
            Debug.Log("InteractiveMessageUI is missing");

        if (currentTransientObjectPortal != null)
        {
            currentTransientObjectPortal.CanUsePortal = currentTransientTempCanUse;
            currentTransientObjectPortal = null;
        }

        isRotating = false;
        playerReferences.IsInteractingRotatingObject = isRotating;
    }

    private void MoveObject()
    {
        if (Vector3.Distance(currentObject.transform.position, holdArea.position) > moveDirectionThreshold)
        {
            Vector3 moveDirection = (holdArea.position - currentObject.transform.position);
            currentObjectBody.AddForce(moveDirection * pickUpForce);
        }
    }

    private bool ActiveToolState()
    {
        if (playerReferences != null)
        {
            if (playerReferences.CurrentToolState == ToolState.HoldTool)
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;

        if (context.started)
        {
            if (!holdingObject)
            {
                UseHoldTool();
            }
            else
            {
                isThrowing = true;
                holdTimer = 0f;
                if (InteractiveMessageUI.Instance)
                    InteractiveMessageUI.Instance.ResetChargeSlider();
                else
                    Debug.Log("InteractiveMessageUI is missing");
            }
        }
        else if (context.canceled && holdingObject)
        {
            if (isThrowing)
            {
                if (holdTimer >= throwThreshold)
                {
                    ThrowObject(holdTimer / maxChargeTime);
                }
                else
                {
                    DropObject();
                }
                isThrowing = false;
                holdTimer = 0f;
                holdTimer = 0f;
                if (InteractiveMessageUI.Instance)
                    InteractiveMessageUI.Instance.ResetChargeSlider();
                else
                    Debug.Log("InteractiveMessageUI is missing");
            }
        }
    }


    private void ThrowObject(float power = 1f)
    {
        if (currentObjectBody != null)
        {
            float throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, power);
            currentObjectBody.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            DropObject();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!ActiveToolState() || playerReferences.CurrentState != PlayerState.Playing) return;

        if (context.started && holdingObject)
        {
            isRotating = true;


        }
        else if (context.canceled)
        {
            isRotating = false;
        }

        playerReferences.IsInteractingRotatingObject = isRotating;
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        this.rotationPos = playerReferences.PlayerLook.LookPos;
    }

    private void RotateObject()
    {
        if (currentObjectBody == null) return;
        float mouseX = rotationPos.x * rotationSpeed * Time.deltaTime;
        float mouseY = rotationPos.y * rotationSpeed * Time.deltaTime;

        currentObjectBody.transform.Rotate(Camera.main.transform.up, -mouseX, Space.World);
        currentObjectBody.transform.Rotate(Camera.main.transform.right, mouseY, Space.World);
    }


}


