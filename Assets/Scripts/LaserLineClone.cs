using Knife.Portal;
using System.Collections.Generic;
using System.Linq;
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
    [Header("Laser Color Settings")]
    [SerializeField] protected Material defaultColor;
    [SerializeField] protected Material currentColor;
    [SerializeField] protected ColorType defaultColorType;
    [SerializeField] protected ColorType currentColorType;
    [SerializeField] private ColorLaserMaterial[] mainColors;
    [SerializeField] private ParticleSystem startParticle;
    [SerializeField] private ParticleSystem endParticle;
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
    public Material CurrentColor { get => currentColor; set => currentColor = value; }
    public ColorType CurrentColorType { get => currentColorType; set => currentColorType = value; }


    private void Start()
    {
        CreateLaserCloneChild();
        ChangeCurrentColorByType(currentColorType);

    }
    public void ChangeCurrentColorByMaterial(ColorLaserMaterial color)
    {
        CurrentColor = color.material;
        CurrentColorType = color.colorType;
        ChangeParticleColors(color.material.color);
        ChangeLaserColor();
    }

    public void ChangeCurrentColorByType(ColorType type)
    {
        ChangeCurrentColorByMaterial(mainColors.FirstOrDefault(cm => cm.colorType == type));
    }
    public void ChangeLaserColor()
    {
        lineRenderer.material = CurrentColor;
    }

    private void ChangeParticleColors(Color newColor)
    {

        startParticle.Stop();
        startParticle.Clear();

        var mainStartModule = startParticle.main;
        mainStartModule.startColor = newColor;

        var mainEndModule = endParticle.main;
        mainEndModule.startColor = newColor;


        startParticle.Play();

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
            if (laserCloneChild.CurrentColorType != GetMixedColor(currentColorType, mirror.MirrorColorType))
            {
                laserCloneChild.CurrentColorType = GetMixedColor(currentColorType, mirror.MirrorColorType);
                laserCloneChild.ChangeCurrentColorByType(laserCloneChild.CurrentColorType);
            }
            laserCloneChild.LaserStartPoint.position = laserEndPoint.position;
            laserCloneChild.CurrentLaserDirection = Vector3.Reflect(currentLaserDirection + laserDirectionOffset, hit.normal);

            return true;
        }
        return false;
    }

    ColorType GetMixedColor(ColorType currentColorType, ColorType mirrorColorType)
    {
        if (mirrorColorType == ColorType.Transparent)
        {
            return currentColorType;
        }
        switch (currentColorType)
        {
            case ColorType.Red:
                if (mirrorColorType == ColorType.Green) return ColorType.Yellow;
                if (mirrorColorType == ColorType.Blue) return ColorType.Magenta;
                break;
            case ColorType.Green:
                if (mirrorColorType == ColorType.Red) return ColorType.Yellow;
                if (mirrorColorType == ColorType.Blue) return ColorType.Cyan;
                break;
            case ColorType.Blue:
                if (mirrorColorType == ColorType.Red) return ColorType.Magenta;
                if (mirrorColorType == ColorType.Green) return ColorType.Cyan;
                break;
                // Add cases for other primary and secondary colors as necessary
        }

        // For simplicity, if no mix is defined, return the mirror's color
        // This can be changed to handle more complex color mixing logic
        return mirrorColorType;
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
            if (laserCloneChild.CurrentColorType != currentColorType)
            {
                laserCloneChild.CurrentColorType = currentColorType;
                laserCloneChild.ChangeCurrentColorByType(laserCloneChild.CurrentColorType);
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