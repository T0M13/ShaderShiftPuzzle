using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractableTool : MonoBehaviour
{

    [Header("References")]
    [ShowOnly][SerializeField] private PlayerReferences playerReferences;
    [ShowOnly][SerializeField] private GameObject currentInteractableObject;
    [ShowOnly][SerializeField] private string lastObjectLayer = "";
    [ShowOnly][SerializeField] private string outlineLayer = "Outline";
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
        if (playerReferences.CurrentState != PlayerState.Playing) return;

        UpdateMessageInteractions();
        UpdatePressMessageInteractions();
        UpdateOutline();
    }

    private void UpdateOutline()
    {
        Ray rayposition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit rayhit;
        bool hitSomething = Physics.Raycast(rayposition, out rayhit, rayDistance, layerMask, QueryTriggerInteraction.Ignore);

        if (hitSomething && rayhit.transform.GetComponent<IInteractable>() != null && rayhit.transform.GetComponent<IInteractable>().ShowOutline() && rayhit.transform.gameObject != playerReferences.PlayerHoldTool.HoldingObject)
        {
            if (rayhit.transform.gameObject != currentInteractableObject)
            {
                if (currentInteractableObject != null)
                {
                    SwapLayer(currentInteractableObject, lastObjectLayer, false);
                }

                currentInteractableObject = rayhit.transform.gameObject;
                lastObjectLayer = GetLayerNameFromGameObject(currentInteractableObject);

                SwapLayer(currentInteractableObject, outlineLayer, false);
            }
        }
        else if (currentInteractableObject != null)
        {
            SwapLayer(currentInteractableObject, lastObjectLayer, false);
            currentInteractableObject = null;
            lastObjectLayer = "";
        }
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

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (playerReferences.CurrentState != PlayerState.Playing) return;
        if (!value.performed) return;
        if (playerReferences.PlayerHoldTool.HoldingObject) return;

        Ray rayposition = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit rayhit;
        if (Physics.Raycast(rayposition, out rayhit, interactDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            IInteractable interactable = rayhit.transform.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanIInteract())
            {
                rayhit.transform.GetComponent<IInteractable>().Interact(playerReferences);
            }
        }
    }

    public string GetLayerNameFromGameObject(GameObject obj)
    {
        string layerName = LayerMask.LayerToName(obj.layer);
        return layerName;
    }

    private void SwapLayer(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
    }

    private void SwapLayer(GameObject obj, string layerName, bool children)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
        if (children)
        {
            foreach (Transform t in obj.transform)
            {
                SwapLayer(t.gameObject, layerName, children);
            }
        }
    }

}
