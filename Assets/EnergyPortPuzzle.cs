using UnityEngine;
using UnityEngine.Events;

public class EnergyPortPuzzle : MonoBehaviour, IEnergy
{
    [SerializeField]
    protected UnityEvent onEnergyReceived;

    [SerializeField]
    protected UnityEvent onEnergyGone;

    public bool isEnergyOn = false;
    public bool needsToStay = true;


    public void OffEnergy()
    {
        if(!needsToStay) return;
        if (isEnergyOn)
        {
            isEnergyOn = false;
            onEnergyGone.Invoke();
        }
    }

    public void OnEnergy()
    {
        if (!isEnergyOn)
        {
            isEnergyOn = true;
            onEnergyReceived.Invoke();
        }
    }

    public bool IsEnergyOn()
    {
        return isEnergyOn;
    }
}
