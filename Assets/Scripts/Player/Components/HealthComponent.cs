using System;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthBehaviour", menuName = "Behaviours/HealthBehaviour")]
public class HealthComponent : ScriptableObject, HealthBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool isDead = false;

    public Action OnDeath;

    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }


    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void GetDamage(float value)
    {
        if (isDead) return;
        CurrentHealth -= value;
        IsDead();
    }

    public void RegainHealth(float value)
    {
        CurrentHealth += value;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void Revive(float value)
    {
        CurrentHealth += value;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        isDead = false;
    }

    public bool IsDead()
    {
        if (CurrentHealth <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
        else
        {
            isDead = false;
        }

        return isDead;
    }
}