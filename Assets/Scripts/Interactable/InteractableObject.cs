using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool outline = false;
    [SerializeField] protected string message = "";
    [SerializeField] protected string pressButtonMessage = "";
    //[SerializeField] protected string taskMessage = "";

    public bool CanInteract { get => canInteract; set => canInteract = value; }
    public bool ShowOutlineValue { get => outline; set => outline = value; }

    public virtual bool CanIInteract()
    {
        return CanInteract;
    }

    public virtual bool ShowOutline()
    {
        return ShowOutlineValue;
    }

    public virtual void Interact(PlayerReferences playerRef)
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

    //public void ShowTaskButtonMessage()
    //{
    //    if (InteractiveMessageUI.Instance)
    //        InteractiveMessageUI.Instance.ChangeTaskMessage(taskMessage);
    //    else
    //        Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    //}


    public void SetEmpty()
    {
        canInteract = false;
        outline = false;
        message = "";
        pressButtonMessage = "";
    }
  
}
