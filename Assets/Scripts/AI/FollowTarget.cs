using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void OnValidate()
    {
        Follow();
    }

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        if (target != null)
            transform.position = target.position + offset;
    }


}
