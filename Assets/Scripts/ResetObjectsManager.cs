using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResetObjectsManager : MonoBehaviour
{
    [SerializeField] private HoldableObject[] holdableObjects;
    [SerializeField] private bool useList = false;
    [SerializeField] private bool isActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (useList)
        {
            if (holdableObjects.Contains(other.GetComponent<HoldableObject>()))
            {
                HoldableObject currentHoldableObject = other.GetComponent<HoldableObject>();
                currentHoldableObject.Respawn();
            }
        }
        else
        {
            if (other.GetComponent<HoldableObject>())
            {
                HoldableObject currentHoldableObject = other.GetComponent<HoldableObject>();
                currentHoldableObject.Respawn();
            }
        }
    }

    public void SetActive()
    {
        isActive = true;
    }

    public void SetInactive()
    {
        isActive = false;
    }

}
