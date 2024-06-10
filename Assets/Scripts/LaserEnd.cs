using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnd : MonoBehaviour
{
    [SerializeField] private LaserLine laserParent;
    [SerializeField] private LaserLineClone laserCloneParent;

    public LaserLine LaserParent { get => laserParent; set => laserParent = value; }
    public LaserLineClone LaserCloneParent { get => laserCloneParent; set => laserCloneParent = value; }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IEnergy>() != null)
        {
            other.GetComponent<IEnergy>().OnEnergy(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IEnergy>() != null)
        {
            other.GetComponent<IEnergy>().OnEnergy(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IEnergy>() != null)
        {
            other.GetComponent<IEnergy>().OffEnergy(this);
        }
    }
}
