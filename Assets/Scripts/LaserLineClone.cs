using Knife.Portal;
using System.Collections.Generic;
using UnityEngine;

public class LaserLineClone : MonoBehaviour
{
    [Header("This Laser References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private Transform laserEndPoint;
    [Header("Laser Clone Child References")]
    [SerializeField] private GameObject laserClonePrefab;
    [SerializeField] private LaserLine laserCloneParent;
    [SerializeField] private Transform laserOtherClonesParent;
    [SerializeField] private LaserLineClone laserCloneChild;
    [Header("Laser Settings")]
    [SerializeField] private bool laserOn = true;
    [SerializeField] private float maxStepDistance = 200;
    [ShowOnly][SerializeField] private float currentStepDistance = 0;
    [SerializeField] private Vector3 laserDirectionOffset;
    [ShowOnly][SerializeField] private Vector3 currentLaserDirection;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private LayerMask layerMask;

    public LaserLine LaserCloneParent { get => laserCloneParent; set => laserCloneParent = value; }
    public Transform LaserStartPoint { get => laserStartPoint; set => laserStartPoint = value; }
    public Vector3 CurrentLaserDirection { get => currentLaserDirection; set => currentLaserDirection = value; }

    private void Start()
    {
        CreateLaserCloneChild();
    }

    private void CreateLaserCloneChild()
    {
        if(laserCloneParent.CurrentReflectionCount < laserCloneParent.MaxReflectionCount)
        {
            GameObject newCloneChild = Instantiate(laserCloneParent.LaserClonePrefab, laserOtherClonesParent);
            laserCloneChild = newCloneChild.GetComponent<LaserLineClone>();
            laserCloneChild.LaserCloneParent = laserCloneParent;
            laserCloneParent.LaserChildren.Add(laserCloneChild);
            laserCloneParent.CurrentReflectionCount++;
        }
    }


    public virtual void Update()
    {
        if (!laserOn) return;
        EmitLaser();

    }

    public virtual void EmitLaser()
    {
        Vector3[] laserPath = CalculateLaserPath();
        if (laserPath.Length > 0)
        {
            lineRenderer.positionCount = laserPath.Length;
            lineRenderer.SetPositions(laserPath);

            if (laserEndPoint != null)
            {
                laserEndPoint.position = laserPath[laserPath.Length - 1];
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (!laserOn || LaserStartPoint == null) return;

        Vector3[] laserPath = CalculateLaserPath();
        if (laserPath.Length < 2) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(laserPath[0], laserPath[1]);

        Gizmos.DrawWireSphere(laserPath[0], 0.1f);
        Gizmos.DrawWireSphere(laserPath[1], 0.1f);
    }

    protected virtual Vector3[] CalculateLaserPath()
    {
        Vector3 currentPosition = LaserStartPoint.position;
        Vector3 laserDirection = CurrentLaserDirection;
        Ray ray = new Ray(currentPosition, laserDirection);

        if (Physics.Raycast(ray, out RaycastHit hit, maxStepDistance, layerMask))
        {
            LaserHits(hit);
            return new Vector3[] { currentPosition, hit.point };
        }
        else
        {
            // If no hit, project the laser to the maximum distance
            return new Vector3[] { currentPosition, currentPosition + laserDirection * maxStepDistance };
        }
    }


    protected virtual void LaserHits(RaycastHit hit)
    {
        //Mirror
        MirrorHit(hit);
    }

    protected virtual void MirrorHit(RaycastHit hit)
    {
        if (laserCloneChild == null) return;
        var mirror = hit.collider.GetComponent<MirrorReflectionObject>();
        if (mirror != null)
        {
            if (!laserCloneChild.gameObject.activeInHierarchy)
            {
                laserCloneChild.gameObject.SetActive(true);
                laserCloneChild.LaserStartPoint = laserEndPoint;
            }
            laserCloneChild.transform.position = hit.point;
            laserCloneChild.CurrentLaserDirection = Vector3.Reflect(currentLaserDirection + laserDirectionOffset, hit.normal);
        }
        else
        {
            if (laserCloneChild.gameObject.activeInHierarchy)
            {
                laserCloneChild.gameObject.SetActive(false);
            }
        }
    }

}
