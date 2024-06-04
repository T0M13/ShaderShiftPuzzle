using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableObject
{

    [SerializeField] private Vector3 closedDoorRotation;
    [SerializeField] private Vector3 openDoorRotation;
    [SerializeField] private bool isDoorOpen = false;
    [SerializeField] private bool interactOnce = false;
    [SerializeField] private float speedMultiplier = 1.0f;

    public override void Interact(PlayerReferences playerRef)
    {
        if (!CanInteract) return;

        base.Interact(playerRef);

        StopAllCoroutines();
        StartCoroutine(AdjustDoorRotation());
    }


    public void OpenDoor()
    {
        if (!CanInteract) return;

        StopAllCoroutines();
        StartCoroutine(AdjustDoorRotation());
    }

    private IEnumerator AdjustDoorRotation()
    {
        float time = 0;
        float duration = 1.0f;
        Vector3 startRotation = transform.localEulerAngles;
        Vector3 endRotation;

        if (isDoorOpen)
        {
            endRotation = closedDoorRotation;
        }
        else
        {
            endRotation = openDoorRotation;
        }

        while (time < duration)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, (time / duration) * speedMultiplier);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = endRotation;
        isDoorOpen = !isDoorOpen;
        if (interactOnce)
        {
            CanInteract = false;
            ShowOutlineValue = false;
            message = "";
        }
    }
}
