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
        if (laserCloneParent.CurrentReflectionCount < laserCloneParent.MaxReflectionCount)
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
        if (laserCloneChild == null) return;

        bool mirrorHit = MirrorHit(hit);
        bool portalHit = PortalHit(hit);

        if (laserCloneChild.gameObject.activeInHierarchy && !mirrorHit && !portalHit)
        {
            laserCloneChild.gameObject.SetActive(false);
        }
    }

    protected virtual bool MirrorHit(RaycastHit hit)
    {
        var mirror = hit.collider.GetComponent<MirrorReflectionObject>();
        if (mirror != null)
        {
            if (!laserCloneChild.gameObject.activeInHierarchy)
            {
                laserCloneChild.gameObject.SetActive(true);
            }
            laserCloneChild.LaserStartPoint.position = laserEndPoint.position;
            laserCloneChild.CurrentLaserDirection = Vector3.Reflect(currentLaserDirection + laserDirectionOffset, hit.normal);

            return true;
        }
        return false;
    }


    protected virtual bool PortalHit(RaycastHit hit)
    {
        var portalMesh = hit.collider.GetComponent<PortalMesh>();
        if (portalMesh != null)
        {
            if (!laserCloneChild.gameObject.activeInHierarchy)
            {
                laserCloneChild.gameObject.SetActive(true);
            }

            var portalTrans = portalMesh.OtherPortalTransition;
            float distanceY = portalMesh.MeshCollider.transform.position.y - hit.point.y;
            float distanceX = portalMesh.MeshCollider.transform.position.x - hit.point.x;
            if (Mathf.Abs(distanceY) > portalMesh.MeshCollider.bounds.size.y / 2)
            {
                distanceY *= -1;
            }
            if (Mathf.Abs(distanceX) > portalMesh.MeshCollider.bounds.size.x / 2)
            {
                distanceX *= -1;
            }
            Vector3 newPos = new Vector3(portalTrans.PortalMesh.MeshCollider.transform.position.x - distanceX, portalTrans.PortalMesh.MeshCollider.transform.position.y - distanceY, portalTrans.PortalMesh.MeshCollider.transform.position.z);
            laserCloneChild.LaserStartPoint.position = newPos + (-portalTrans.PortalMesh.MeshCollider.transform.forward * laserCloneParent.LaserPortalOffset);
            laserCloneChild.CurrentLaserDirection = currentLaserDirection + laserDirectionOffset;
            return true;
        }
        return false;
    }
}