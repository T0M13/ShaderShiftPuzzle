using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractableTool : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [Header("Settings")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance = 7;
    [SerializeField] private float interactDistance = 1;

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
        UpdateMessageInteractions();
        UpdatePressMessageInteractions();
    }

    private void UpdateMessageInteractions()
    {
        Ray rayposition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit rayhit;
        if (Physics.Raycast(rayposition, out rayhit, rayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (rayhit.transform.GetComponent<IInteractable>() != null)
            {
                rayhit.transform.GetComponent<IInteractable>().ShowMessage();
            }
            else
            {
                if (InteractiveMessageUI.Instance)
                    InteractiveMessageUI.Instance.ChangeMessage("");
            }
        }
        else
        {
            if (InteractiveMessageUI.Instance)
                InteractiveMessageUI.Instance.ChangeMessage("");
        }
    }

    private void UpdatePressMessageInteractions()
    {
        if (playerReferences.PlayerHoldTool.HoldingObject)
        {
            if (InteractiveMessageUI.Instance)
                InteractiveMessageUI.Instance.ChangePressMessage("");

            return;
        }

        Ray rayposition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit rayhit;
        if (Physics.Raycast(rayposition, out rayhit, interactDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (rayhit.transform.GetComponent<IInteractable>() != null && rayhit.transform.GetComponent<IInteractable>().CanIInteract())
            {
                rayhit.transform.GetComponent<IInteractable>().ShowUseButtonMessage();
            }
            else
            {
                if (InteractiveMessageUI.Instance)
                    InteractiveMessageUI.Instance.ChangePressMessage("");
            }
        }
        else
        {
            if (InteractiveMessageUI.Instance)
                InteractiveMessageUI.Instance.ChangePressMessage("");
        }
    }

    private void OnInteract(InputValue value)
    {
        if (playerReferences.PlayerHoldTool.HoldingObject) return;

        Ray rayposition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit rayhit;
        if (Physics.Raycast(rayposition, out rayhit, interactDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (rayhit.transform.GetComponent<IInteractable>() != null && rayhit.transform.GetComponent<IInteractable>().CanIInteract())
            {
                rayhit.transform.GetComponent<IInteractable>().Interact();
            }
        }
    }


}
