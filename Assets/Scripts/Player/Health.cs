using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent { get => healthComponent; set => healthComponent = value; }
}
