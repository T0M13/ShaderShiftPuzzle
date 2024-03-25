using Knife.Portal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTransientObject : MonoBehaviour, IPortalTransient
{

    [SerializeField] private bool canUsePortal = true;

    public Vector3 Position => transform.position;

    public bool CanUsePortal { get => canUsePortal; set => canUsePortal = value; }

    public void Teleport(Vector3 position, Quaternion rotation, Transform entry, Transform exit)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
