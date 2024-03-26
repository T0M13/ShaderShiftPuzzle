using UnityEngine;

public class ProximityActivator : MonoBehaviour
{
    [Header("Settings")]
    public float activationDistance = 10f;
    public GameObject[] objectsToActivate;
    public bool showGizmos = true;

    void Update()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) <= activationDistance)
            {
                if (!obj.activeInHierarchy)
                    obj.SetActive(true);
            }
            else
            {
                if (obj.activeInHierarchy)
                    obj.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}
