using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private bool canInteract = true;
    [SerializeField] protected string message = "";
    [SerializeField] protected string pressButtonMessage = "";

    public bool CanInteract { get => canInteract; set => canInteract = value; }

    public virtual bool CanIInteract()
    {
        return CanInteract;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with this object. " + "ObjectName: " + gameObject.name);
    }

    public virtual void ShowMessage()
    {
        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ChangeMessage(message);
        else
            Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    }

    public void ShowUseButtonMessage()
    {
        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ChangePressMessage(pressButtonMessage);
        else
            Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    }
}
