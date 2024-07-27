using UnityEngine;

public class DistanceBasedDistortion : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Material distortionMaterial; // Reference to the material with the distortion shader
    public float maxDistortion = 0.4f; // Maximum distortion value
    public float minDistortion = 0.0f; // Minimum distortion value
    public float maxDistance = 10.0f; // Distance at which distortion is max

    private void Update()
    {
        if (player != null && distortionMaterial != null)
        {
            // Calculate the distance between the player and this game object
            float distance = Vector3.Distance(player.position, transform.position);

            // Normalize the distance to a value between 0 and 1
            float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

            // Calculate the distortion value based on the normalized distance
            float distortionValue = Mathf.Lerp(minDistortion, maxDistortion, normalizedDistance);

            // Set the distortion value in the material
            distortionMaterial.SetFloat("_DistortionAmount", distortionValue);
        }
    }
}
