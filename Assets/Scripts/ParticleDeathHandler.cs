using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDeathHandler : MonoBehaviour
{
    [Header("Settings")]
    public bool onlyDeactivate;

    [Header("ParentMode")]
    public Transform parent;
    public bool parentBack;

    void OnEnable()
    {
        StartCoroutine("CheckIfAlive");
    }

    IEnumerator CheckIfAlive()
    {
        ParticleSystem ps = this.GetComponent<ParticleSystem>();

        while (true && ps != null)
        {
            yield return new WaitForSeconds(0.5f);
            if (!ps.IsAlive(true))
            {
                if (onlyDeactivate)
                {
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
                    if (parent != null && onlyDeactivate && parentBack)
                    {
                        this.transform.parent = parent;
                    }
                    this.gameObject.SetActive(false);
#endif
                }
                else
                    GameObject.Destroy(this.gameObject);
                break;
            }
        }
    }
}
