using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFire : MonoBehaviour
{
    [SerializeField] private Vector2 startTimeMinAndMax;

    private void Start()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySound("Fire", gameObject, startTimeMinAndMax.x, startTimeMinAndMax.y);
        }
    }
}
