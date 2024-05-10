using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpinner : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * speed);
    }
}
