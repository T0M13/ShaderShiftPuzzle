using UnityEngine;

public interface IInteractable
{
    public void Interact(PlayerReferences playerRef);
    public bool CanIInteract();
    public bool ShowOutline();
    public void ShowMessage();
    public void ShowUseButtonMessage();
    public bool CanShowPressButtonMessage();
}

