using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKeyObject : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private HoldableObject thisHoldableObject;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float lerpSpeed = 5.0f;
    [SerializeField] private bool startLerp = false;
    [SerializeField] private bool setInTargetAsChild = false;
    private void Update()
    {
        LerpingToPosition();
    }

    private void LerpingToPosition()
    {
        if (startLerp)
        {
            // Interpolate position
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, lerpSpeed * Time.deltaTime);

            // Interpolate rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetPosition.rotation, lerpSpeed * Time.deltaTime);

            // Interpolate scale
            transform.localScale = Vector3.Lerp(transform.localScale, targetPosition.localScale, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetPosition.rotation) < 1.0f)
            {
                transform.position = targetPosition.position;
                transform.rotation = targetPosition.rotation;
                transform.localScale = targetPosition.localScale;
                if (setInTargetAsChild)
                {
                    transform.SetParent(targetPosition, false);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = Vector3.one;
                }
                startLerp = false;
            }
        }
    }

    public void LerpToPosition()
    {
        if (thisHoldableObject.playerHoldToolRef != null)
        {
            thisHoldableObject.playerHoldToolRef.DropObject();
        }
        thisHoldableObject.canBeHeld = false;

        boxCollider.enabled = false;
        rbody.useGravity = false;
        startLerp = true;
    }

    public void ReactivateKeyObject()
    {
        boxCollider.enabled = true;
        rbody.useGravity = true;
    }
}
