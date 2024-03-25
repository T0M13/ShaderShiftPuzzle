using Knife.Portal;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserStartPoint;
    public GameObject laserPointPrefab;
    public Transform laserPointsParent;
    public List<Transform> laserPoints;
    public GameObject endMarker;
    public bool laserOn = true;
    [ShowOnly][SerializeField] private bool mainLaser = true;
    [SerializeField] private int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    public Vector3 lastPosition;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private VolumeDirection volumeDirection = VolumeDirection.Z;
    [ShowOnly][SerializeField] private Vector3[] laserPath;

    [SerializeField] private LaserLine laserPrefab;
    [SerializeField] private List<LaserThroughPortal> lasersThroughPortal;

    private enum VolumeDirection { X, Y, Z }

    public int MaxReflectionCount { get => maxReflectionCount; set => maxReflectionCount = value; }
    public List<LaserThroughPortal> LasersThroughPortal { get => lasersThroughPortal; set => lasersThroughPortal = value; }
    public bool MainLaser { get => mainLaser; set => mainLaser = value; }

    void Awake()
    {
        InitializePoints();
    }

    void InitializePoints()
    {
        for (int i = 0; i < MaxReflectionCount; i++)
        {
            GameObject point = Instantiate(laserPointPrefab, laserPointsParent);
            point.SetActive(false);
            laserPoints.Add(point.transform);
        }
    }

    void Update()
    {
        if (!laserOn) return;
        EmitLaser();

    }

    void OnDrawGizmos()
    {
        DrawLaserGizmo();
    }

    public void EmitLaser()
    {
        laserPath = CalculateLaserPath();
        if (laserPath.Length > 0)
        {
            lineRenderer.positionCount = laserPath.Length;
            lineRenderer.SetPositions(laserPath);

            if (endMarker != null)
            {
                lastPosition = laserPath[laserPath.Length - 1];
                endMarker.transform.position = lastPosition;

            }
        }
    }

    void DrawLaserGizmo()
    {
        Vector3[] laserPath = CalculateLaserPath();
        for (int i = 0; i < laserPath.Length - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(laserPath[i], laserPath[i + 1]);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(laserStartPoint.position, .1f);

        if (laserPath.Length > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(laserPath[laserPath.Length - 1], 0.1f);
        }

    }
    Vector3[] CalculateLaserPath()
    {
        var path = new List<Vector3>();
        int reflectionCount = 0;
        float stepDistance = 0;
        Vector3 laserDirection = laserStartPoint.forward;
        Vector3 currentPosition = laserStartPoint.position;
        path.Add(currentPosition);

        // Deactivate all points initially
        foreach (Transform point in laserPoints)
        {
            point.gameObject.SetActive(false);
        }

        while (reflectionCount < MaxReflectionCount && stepDistance < maxStepDistance)
        {
            Ray ray = new Ray(currentPosition, laserDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, maxStepDistance - stepDistance, layerMask))
            {
                stepDistance += hit.distance;
                currentPosition = hit.point;
                path.Add(currentPosition);

                // Activate and position point
                if (reflectionCount < laserPoints.Count)
                {
                    Transform hitPointTransform = laserPoints[reflectionCount];
                    hitPointTransform.position = currentPosition;
                    hitPointTransform.gameObject.SetActive(true);
                }


                //Portal
                var portalMesh = hit.collider.GetComponent<PortalMesh>();
                if (portalMesh != null && MainLaser)
                {
                    var portalTrans = portalMesh.OtherPortalTransition;
                    if (LasersThroughPortal.Count > 0)
                    {
                        foreach (var laser in LasersThroughPortal)
                        {
                            if (laser.portal != portalTrans)
                            {
                                CreateNewLaser(portalTrans, reflectionCount);
                            }
                            else
                            {
                                if (!laser.newLaser.gameObject.activeSelf)
                                    laser.newLaser.gameObject.SetActive(true);


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


                                laser.newLaser.laserStartPoint.position = newPos;

                                laser.newLaser.laserStartPoint.forward = laserDirection;
                                laser.newLaser.EmitLaser();

                            }
                        }
                    }
                    else
                    {
                        CreateNewLaser(portalTrans, reflectionCount);
                    }
                }
                else
                {
                    foreach (var laser in LasersThroughPortal)
                    {
                        if (laser.newLaser.gameObject.activeSelf)
                            laser.newLaser.gameObject.SetActive(false);
                    }
                }

                var mirror = hit.collider.GetComponent<MirrorReflectionObject>();
                if (mirror != null)
                {
                    reflectionCount++;
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                }
                else
                {
                    break;
                }
            }
            else
            {
                path.Add(currentPosition + laserDirection * (maxStepDistance - stepDistance));
                break;
            }
        }

        return path.ToArray();
    }

    private void CreateNewLaser(PortalTransition portalTrans, int reflectionCount)
    {
        var newLaserTemp = Instantiate(laserPrefab);
        var newLaserThrough = new LaserThroughPortal(this, newLaserTemp, portalTrans);
        LasersThroughPortal.Add(newLaserThrough);
        newLaserTemp.maxReflectionCount = newLaserThrough.newLaser.maxReflectionCount - reflectionCount;
        newLaserTemp.MainLaser = false;
    }


    [System.Serializable]
    public class LaserThroughPortal
    {
        public LaserLine oldLaser;
        public LaserLine newLaser;
        public PortalTransition portal;

        public LaserThroughPortal(LaserLine oldLaser, LaserLine newLaser, PortalTransition portal)
        {
            this.oldLaser = oldLaser;
            this.newLaser = newLaser;
            this.portal = portal;
        }



    }

}
