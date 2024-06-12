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


    public void OffEnergy(LaserEnd laserEnd)
    {
        if (!needsToStay) return;
        if (isEnergyOn)
        {
            isEnergyOn = false;
            onEnergyGone?.Invoke();
        }
    }

    public void OnEnergy(LaserEnd laserEnd)
    {
        if (!isEnergyOn)
        {
            isEnergyOn = true;
            onEnergyReceived?.Invoke();
        }
    }

    protected virtual bool IsEnergyOn()
    {
        return isEnergyOn;
    }
}
