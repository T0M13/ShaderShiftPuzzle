using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public bool CanIInteract();
    public void ShowMessage();
    public void ShowUseButtonMessage();
}

