using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleButton : InteractableObject
{
    [SerializeField] private bool oneTimeInteraction = false;
    public UnityEvent onInteract;
    public override void Interact(PlayerReferences playerRef)
    {
        if (CanInteract)
        {
            base.Interact(playerRef);
            onInteract?.Invoke();
            if (oneTimeInteraction)
            {
                CanInteract = false;
                ShowOutlineValue = false;
            }
        }
    }

}
