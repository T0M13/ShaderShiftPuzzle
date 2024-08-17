using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleButtonHit : InteractableObject
{
    [SerializeField] private bool oneTimeInteraction = true;
    public UnityEvent onButtonHit;

    [SerializeField] private string soundName;

    private void Start()
    {
        if (!string.IsNullOrEmpty(soundName) && AudioManager.Instance)
        {
            AudioManager.Instance.InitializeSound(soundName, gameObject);
        }
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlaySound(soundName, gameObject);
    }

    public void StopSound()
    {
        AudioManager.Instance.StopSound( gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!CanInteract) return;
        onButtonHit?.Invoke();
        SetEmpty();
    }

    public override void Interact(PlayerReferences playerRef)
    {
        if (!CanInteract) return;
        onButtonHit?.Invoke();
        SetEmpty();
    }


}
