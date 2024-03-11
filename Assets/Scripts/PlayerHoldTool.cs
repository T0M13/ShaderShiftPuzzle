using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldTool : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Transform holdArea;
    [Header("Pick Up Settings")]
    [SerializeField] private float pickUpRange = 15f;
    [SerializeField] private float pickUpForce = 150f;
    [SerializeField] private GameObject currentObject;
    [SerializeField] private Rigidbody currentObjectBody;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance = 6;
    [SerializeField] private bool holdingObject = false;

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


    private void FixedUpdate()
    {
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

        if (gameObject.GetComponent<Rigidbody>())
        {
            currentObjectBody = gameObject.GetComponent<Rigidbody>();
            currentObjectBody.useGravity = false;
            currentObjectBody.drag = 10;
            currentObjectBody.constraints = RigidbodyConstraints.FreezeRotation;

            currentObjectBody.transform.SetParent(holdArea);

            currentObject = gameObject;

            HoldingObject = true;

        }
    }


    private void DropObject()
    {
        currentObjectBody.useGravity = true;
        currentObjectBody.drag = 1;
        currentObjectBody.constraints = RigidbodyConstraints.None;

        currentObjectBody.transform.SetParent(null);
        currentObjectBody = null;
        currentObject = null;

        HoldingObject = false;

    }

    private void MoveObject()
    {
        if (Vector3.Distance(currentObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - currentObject.transform.position);
            currentObjectBody.AddForce(moveDirection * pickUpForce);
        }
    }


    private void OnFire(InputValue value)
    {
        float tempValue = value.Get<float>();
        if (this.enabled)
            UseHoldTool();
    }

}
