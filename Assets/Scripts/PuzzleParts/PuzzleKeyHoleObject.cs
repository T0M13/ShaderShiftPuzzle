using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PuzzleKeyHoleObject : MonoBehaviour
{

    [SerializeField] private PuzzleKeyObject key;

    public UnityEvent onKeyPlaced;
    public UnityEvent onKeyExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PuzzleKeyObject>() == key)
        {
            onKeyPlaced?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PuzzleKeyObject>() == key)
        {
            onKeyExited?.Invoke();
        }
    }


}
