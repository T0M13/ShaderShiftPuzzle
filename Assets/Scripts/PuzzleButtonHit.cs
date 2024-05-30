using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleButtonHit : InteractableObject
{
    [SerializeField] private bool oneTimeInteraction = true;
    public UnityEvent onButtonHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (!CanInteract) return;
        onButtonHit?.Invoke();
        SetEmpty();
    }

    public override void Interact(PlayerReferences playerRef)
    {
        if (!CanInteract) return;
        onButtonHit?.Invoke();
        SetEmpty();
    }


}
