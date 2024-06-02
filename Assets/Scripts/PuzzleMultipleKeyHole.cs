using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleMultipleKeyHole : MonoBehaviour
{
    [SerializeField] private int activationsNeeded = 2;
    [SerializeField] private int currentActivations = 0;

    public UnityEvent onAllActivationsDone;
    public UnityEvent onDectivation;

    public void OnActivation()
    {
        currentActivations++;
        if (currentActivations >= activationsNeeded)
        {
            currentActivations = activationsNeeded;
            onAllActivationsDone?.Invoke();

        }
    }

    public void OnDeactivation()
    {
        currentActivations--;
        if (currentActivations <= 0)
        {
            currentActivations = 0;
            onDectivation?.Invoke();

        }
    }

}
