using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;


    private void OnEnable()
    {
        playerReferences.PlayerHealth.HealthComponent.OnDeath += Death;
    }
    private void OnDisable()
    {
        playerReferences.PlayerHealth.HealthComponent.OnDeath -= Death;
    }

    private void Awake()
    {
        GetReferences();
        Revive();
    }

    private void GetReferences()
    {
        if (playerReferences == null)
            playerReferences = GetComponent<PlayerReferences>();
    }

    private void Death()
    {
        Debug.Log("You Died");
    }

    private void Revive()
    {
        playerReferences.PlayerHealth.HealthComponent.Revive(playerReferences.PlayerHealth.HealthComponent.MaxHealth);
    }


}
