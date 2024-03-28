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
    //[SerializeField] protected List<LaserThroughPortal> lasersThroughPortal;
    //public List<LaserThroughPortal> LasersThroughPortal { get => lasersThroughPortal; set => lasersThroughPortal = value; }
    public bool MainLaser { get => mainLaser; set => mainLaser = value; }
    public int CurrentReflectionCount { get => currentReflectionCount; set => currentReflectionCount = value; }
    public List<LaserLineClone> LaserChildren { get => laserChildren; set => laserChildren = value; }
    public int MaxReflectionCount { get => maxReflectionCount; set => maxReflectionCount = value; }
    public GameObject LaserClonePrefab { get => laserClonePrefab; set => laserClonePrefab = value; }

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
        Vector3 laserDirection = currentLaserDirection;
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

    protected virtual void OnDrawGizmos()
    {
        if (!laserOn || laserStartPoint == null) return;

        Vector3[] laserPath = CalculateLaserPath();
        if (laserPath.Length < 2) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(laserPath[0], laserPath[1]);

        Gizmos.DrawWireSphere(laserPath[0], 0.1f);
        Gizmos.DrawWireSphere(laserPath[1], 0.1f);
    }

    protected virtual void LaserHits(RaycastHit hit)
    {
        //Mirror
        MirrorHit(hit);



        //Portal
        //var portalMesh = hit.collider.GetComponent<PortalMesh>();
        //if (portalMesh != null && MainLaser)
        //{
        //    var portalTrans = portalMesh.OtherPortalTransition;
        //    if (LasersThroughPortal.Count > 0)
        //    {
        //        foreach (var laser in LasersThroughPortal)
        //        {
        //            if (laser.portal != portalTrans)
        //            {
        //                LaserThroughPortalNew(portalTrans, currentReflectionCount);
        //            }
        //            else
        //            {
        //                if (!laser.newLaser.gameObject.activeSelf)
        //                    laser.newLaser.gameObject.SetActive(true);


        //                float distanceY = portalMesh.MeshCollider.transform.position.y - hit.point.y;
        //                float distanceX = portalMesh.MeshCollider.transform.position.x - hit.point.x;
        //                if (Mathf.Abs(distanceY) > portalMesh.MeshCollider.bounds.size.y / 2)
        //                {
        //                    distanceY *= -1;
        //                }
        //                if (Mathf.Abs(distanceX) > portalMesh.MeshCollider.bounds.size.x / 2)
        //                {
        //                    distanceX *= -1;
        //                }
        //                Vector3 newPos = new Vector3(portalTrans.PortalMesh.MeshCollider.transform.position.x - distanceX, portalTrans.PortalMesh.MeshCollider.transform.position.y - distanceY, portalTrans.PortalMesh.MeshCollider.transform.position.z);

        //                laser.newLaser.LaserStartPoint.position = newPos;

        //                laser.newLaser.LaserStartPoint.forward = laserDirection;
        //                laser.newLaser.EmitLaser();

        //            }
        //        }
        //    }
        //    else
        //    {
        //        LaserThroughPortalNew(portalTrans, currentReflectionCount);
        //    }
        //}
        //else
        //{
        //    foreach (var laser in LasersThroughPortal)
        //    {
        //        if (laser.newLaser.gameObject.activeSelf)
        //            laser.newLaser.gameObject.SetActive(false);
        //    }
        //}


    }


    //protected virtual void LaserThroughPortalNew(PortalTransition portalTrans, int reflectionCount)
    //{
    //    LaserLineClone newLaserTemp = GetAClone();
    //    var newLaserThrough = new LaserThroughPortal(this, newLaserTemp, portalTrans);
    //    LasersThroughPortal.Add(newLaserThrough);
    //    newLaserTemp.MaxReflectionCount = 1;
    //}


    //[System.Serializable]
    //public class LaserThroughPortal
    //{
    //    public LaserLine oldLaser;
    //    public LaserLineClone newLaser;
    //    public PortalTransition portal;

    //    public LaserThroughPortal(LaserLine oldLaser, LaserLineClone newLaser, PortalTransition portal)
    //    {
    //        this.oldLaser = oldLaser;
    //        this.newLaser = newLaser;
    //        this.portal = portal;
    //    }
    //}


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
