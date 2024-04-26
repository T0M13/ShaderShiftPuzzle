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

    public UnityEvent<float> OnIncreasePressure;
    public UnityEvent<float> OnDecreasePressure;
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
            float oldPressureWeight = pressureWeightCurrent;
            Rigidbody objectBody = other.GetComponent<Rigidbody>();
            pressureWeightCurrent += objectBody.mass;
            CheckPressure(oldPressureWeight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            float oldPressureWeight = pressureWeightCurrent;
            Rigidbody objectBody = other.GetComponent<Rigidbody>();
            pressureWeightCurrent -= objectBody.mass;
            CheckPressure(oldPressureWeight);
        }
    }

    private void CheckPressure(float oldPressureWeight)
    {

        if (oldPressureWeight < pressureWeightCurrent)
        {
            OnIncreasePressure?.Invoke(pressureWeightCurrent);
        }
        if (oldPressureWeight > pressureWeightCurrent)
        {
            OnDecreasePressure?.Invoke(pressureWeightCurrent);
        }

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
