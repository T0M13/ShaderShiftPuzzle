using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PressurePlate : MonoBehaviour
{

    [SerializeField] private float pressureWeightNeeded = 3f;
    [SerializeField] private float pressureWeightCurrent = 0f;
    [SerializeField] private bool pressureNeedsToStay = false;

    public UnityEvent DoInteractOnPressure;
    public UnityEvent UndoInteractOnPressure;

    private void Awake()
    {
        pressureWeightCurrent = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            Rigidbody objectBody = other.GetComponent<Rigidbody>();
            pressureWeightCurrent += objectBody.mass;
            CheckPressure();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            Rigidbody objectBody = other.GetComponent<Rigidbody>();
            pressureWeightCurrent -= objectBody.mass;
            CheckPressure();
        }
    }

    private void CheckPressure()
    {
        if (pressureWeightCurrent >= pressureWeightNeeded)
        {
            DoInteractOnPressure?.Invoke();
        }
        if (pressureWeightCurrent < pressureWeightNeeded && pressureNeedsToStay)
        {
            UndoInteractOnPressure?.Invoke();
        }
    }

}
