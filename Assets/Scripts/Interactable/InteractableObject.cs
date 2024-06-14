using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool outline = false;
    [SerializeField] private string message = "";
    [SerializeField] private string pressButtonMessage = "";
    //[SerializeField] protected string taskMessage = "";

    public bool CanInteract { get => canInteract; set => canInteract = value; }
    public bool ShowOutlineValue { get => outline; set => outline = value; }
    protected string PressButtonMessage { get => pressButtonMessage; set => pressButtonMessage = value; }
    protected string Message { get => message; set => message = value; }

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
        return;
    }

    public virtual void ShowMessage()
    {
        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ChangeMessage(Message);
        else
            Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    }

    public void ShowUseButtonMessage()
    {
        if (InteractiveMessageUI.Instance)
            InteractiveMessageUI.Instance.ChangePressMessage(PressButtonMessage);
        else
            Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    }

    public void ChangeMessage(string message)
    {
        Message = message;  
    }

    public void ChangePressMessage(string pressButtonMessage)
    {
        PressButtonMessage = pressButtonMessage;
    }


    //public void ShowTaskButtonMessage()
    //{
    //    if (InteractiveMessageUI.Instance)
    //        InteractiveMessageUI.Instance.ChangeTaskMessage(taskMessage);
    //    else
    //        Debug.LogWarning("No InteractiveMessageUI Object in Scene");
    //}


    public virtual void SetEmpty()
    {
        canInteract = false;
        outline = false;
        Message = "";
        PressButtonMessage = "";
        
    }
  
}
