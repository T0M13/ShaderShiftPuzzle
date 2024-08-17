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


    private void Start()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.InitializeSound("ElectricLoopedHigh", gameObject);
        }
    }

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

    public void PlaySound(string name)
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySound(name, gameObject);
        }
    }

    public void StopSound()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.StopSound(gameObject);
        }
    }
}
