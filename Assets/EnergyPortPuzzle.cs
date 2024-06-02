using UnityEngine;
using UnityEngine.Events;

public class EnergyPortPuzzle : MonoBehaviour, IEnergy
{
    [SerializeField]
    private UnityEvent onEnergyReceived;

    [SerializeField]
    private UnityEvent onEnergyGone;

    private bool isEnergyOn = false;

    public void OffEnergy()
    {
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
