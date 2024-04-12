using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnd : MonoBehaviour
{
    [SerializeField] private LaserLine laserParent;
    [SerializeField] private LaserLineClone laserCloneParent;

    public LaserLine LaserParent { get => laserParent; set => laserParent = value; }
    public LaserLineClone LaserCloneParent { get => laserCloneParent; set => laserCloneParent = value; }
}
