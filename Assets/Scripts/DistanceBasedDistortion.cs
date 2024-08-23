using UnityEngine;

public class DistanceBasedDistortion : MonoBehaviour
{
    public Transform player; 
    public Material distortionMaterial;
    public float maxDistortion = 0.4f; 
    public float minDistortion = 0.0f;
    public float maxDistance = 10.0f;

    private void Update()
    {
        if (player != null && distortionMaterial != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

            float distortionValue = Mathf.Lerp(minDistortion, maxDistortion, normalizedDistance);

            distortionMaterial.SetFloat("_DistortionAmount", distortionValue);
        }
    }
}
