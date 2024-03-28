using Knife.Portal;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : MonoBehaviour
{
    [Header("This Laser References")]
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected Transform laserStartPoint;
    [SerializeField] protected Transform laserEndPoint;
    [Header("Laser Clone Child References")]
    [SerializeField] private GameObject laserClonePrefab;
    [SerializeField] protected Transform laserCloneParent;
    [SerializeField] protected LaserLineClone laserCloneChild;
    [SerializeField] private List<LaserLineClone> laserChildren;
    [Header("Laser Settings")]
    [SerializeField] protected bool mainLaser = true;
    [SerializeField] protected bool laserOn = true;
    [SerializeField] private int maxReflectionCount = 5;
    [SerializeField] private int currentReflectionCount = 0;
    [SerializeField] protected float maxStepDistance = 200;
    [ShowOnly][SerializeField] protected float currentStepDistance = 0;
    [SerializeField] protected Vector3 laserDirectionOffset;
    [ShowOnly][SerializeField] protected Vector3 currentLaserDirection;
    [SerializeField] protected Vector3 lastPosition;
    [SerializeField] protected LayerMask layerMask;
    [Header("Laser Portal Settings")]
    [SerializeField] private float laserPortalOffset = 0.01f;


    public bool MainLaser { get => mainLaser; set => mainLaser = value; }
    public int CurrentReflectionCount { get => currentReflectionCount; set => currentReflectionCount = value; }
    public List<LaserLineClone> LaserChildren { get => laserChildren; set => laserChildren = value; }
    public int MaxReflectionCount { get => maxReflectionCount; set => maxReflectionCount = value; }
    public GameObject LaserClonePrefab { get => laserClonePrefab; set => laserClonePrefab = value; }
    public float LaserPortalOffset { get => laserPortalOffset; set => laserPortalOffset = value; }

    protected void Awake()
    {
        CreateLaserCloneChild();
    }


    protected virtual void Update()
    {
        if (!laserOn) return;
        EmitLaser();

    }

    private void CreateLaserCloneChild()
    {
        GameObject newCloneChild = Instantiate(LaserClonePrefab, laserCloneParent);
        laserCloneChild = newCloneChild.GetComponent<LaserLineClone>();
        laserCloneChild.LaserCloneParent = this;
        LaserChildren.Add(laserCloneChild);
        CurrentReflectionCount++;
    }


    protected virtual void EmitLaser()
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

    protected virtual Vector3[] CalculateLaserPath()
    {
        Vector3 currentPosition = laserStartPoint.position;
        Vector3 laserDirection = currentLaserDirection + laserDirectionOffset;
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
            laserCloneChild.LaserStartPoint.position = newPos + (-portalTrans.PortalMesh.MeshCollider.transform.forward * LaserPortalOffset);
            laserCloneChild.CurrentLaserDirection = currentLaserDirection + laserDirectionOffset;
            return true;
        }
        return false;
    }



    #region GIZMO
    /// <summary>
    /// GIZMO
    /// </summary>
    protected void OnDrawGizmos()
    {
        if (!laserOn || laserStartPoint == null) return;

        List<Vector3> laserPath = CalculateLaserPathForGizmos();
        if (laserPath.Count < 2) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < laserPath.Count - 1; i++)
        {
            Gizmos.DrawLine(laserPath[i], laserPath[i + 1]);
            Gizmos.DrawWireSphere(laserPath[i], 0.1f);
        }
        Gizmos.DrawWireSphere(laserPath[laserPath.Count - 1], 0.1f);
    }

    /// <summary>
    /// GIZMO ONLY
    /// </summary>
    /// <returns></returns>
    protected List<Vector3> CalculateLaserPathForGizmos()
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 currentPosition = laserStartPoint.position;
        Vector3 laserDirection = currentLaserDirection + laserDirectionOffset;
        int reflectionCount = 0;

        path.Add(currentPosition); // Add the starting point

        while (reflectionCount < maxReflectionCount)
        {
            Ray ray = new Ray(currentPosition, laserDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, maxStepDistance, layerMask))
            {
                path.Add(hit.point);
                // Check if it hits a mirror, else break
                var mirror = hit.collider.GetComponent<MirrorReflectionObject>();
                if (mirror != null)
                {
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                    currentPosition = hit.point;
                }
                else
                {
                    break; // No more reflections if it's not a mirror
                }
            }
            else
            {
                path.Add(currentPosition + laserDirection * maxStepDistance); // Extend to max distance if no hit
                break; // Exit loop if ray doesn't hit anything
            }

            reflectionCount++;
        }

        return path;
    }
    #endregion
}
