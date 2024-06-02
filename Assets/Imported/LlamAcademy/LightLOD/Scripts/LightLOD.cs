using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LlamAcademy.LightLOD
{
    [RequireComponent(typeof(Light))]
    public class LightLOD : MonoBehaviour
    {
        private Light Light;
        public bool LightShouldBeOn = true;
        [SerializeField]
        private float UpdateDelay = 0.1f;
        [SerializeField]
        private List<LODAdjustment> LODLevels = new();
        [SerializeField]
        private float visibilityMargin = 10.0f;  // Extra space around the camera frustum
        [SerializeField]
        private float forwardRange = 50.0f;      // Forward range from the camera

        // Initial settings
        private bool initialLightEnabled;
        private LightShadows initialLightShadows;
        private LightShadowResolution initialShadowResolution;

        private void Awake()
        {
            Light = GetComponent<Light>();
            // Cache the initial settings
            initialLightEnabled = Light.enabled;
            initialLightShadows = Light.shadows;
            initialShadowResolution = Light.shadowResolution;
        }

        private void OnEnable()
        {
            StartCoroutine(AdjustLODQuality());
        }

        private IEnumerator AdjustLODQuality()
        {
            WaitForSeconds wait = new WaitForSeconds(UpdateDelay);

            while (true)
            {
                if (LightLODCamera.Instance == null || !LightShouldBeOn)
                {
                    Light.enabled = false;
                    yield return wait;
                    continue;
                }

                var cam = LightLODCamera.Instance.GetComponent<Camera>();
                var planes = GeometryUtility.CalculateFrustumPlanes(cam);
                Vector3 extendedFront = cam.transform.position + cam.transform.forward * forwardRange;
                var bounds = new Bounds(extendedFront, Vector3.one * (visibilityMargin + forwardRange));

                bool isVisible = GeometryUtility.TestPlanesAABB(planes, bounds);

                if (isVisible)
                {
                    float squareDistanceFromCamera = (cam.transform.position - transform.position).sqrMagnitude;

                    bool foundLOD = false;
                    foreach (var lod in LODLevels)
                    {
                        if (squareDistanceFromCamera <= lod.MaxSquareDistance)
                        {
                            Light.enabled = true;
                            Light.shadows = lod.LightShadows;
                            Light.shadowResolution = (LightShadowResolution)Mathf.Min((int)lod.ShadowResolution, (int)QualitySettings.shadowResolution);
                            foundLOD = true;
                            break;
                        }
                    }
                    if (!foundLOD)
                    {
                        // Restore to initial settings if no LOD matches
                        Light.enabled = initialLightEnabled;
                        Light.shadows = initialLightShadows;
                        Light.shadowResolution = initialShadowResolution;
                    }
                }
                else
                {
                    Light.enabled = false;
                }

                yield return wait;
            }
        }

        // Optional: Debugging to visualize the camera frustum and extended range
        private void OnDrawGizmos()
        {
            if (LightLODCamera.Instance != null)
            {
                Camera cam = LightLODCamera.Instance.GetComponent<Camera>();
                Gizmos.color = Color.cyan;
                Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
                Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);

                // Draw extended range
                Gizmos.color = Color.blue;
                Vector3 extendedFront = cam.transform.position + cam.transform.forward * forwardRange;
                Gizmos.DrawWireSphere(extendedFront, visibilityMargin);
            }
        }
    }
}
