using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToolManager : MonoBehaviour
{
    [Header("Orb")]
    [SerializeField] private ParticleSystem[] orbParticles;
    [Header("General Settings")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private bool isDefaultColor;
    [Header("Hit Feedback")]
    [SerializeField] private GameObject paintParent;
    [SerializeField] private ParticleSystem[] paintParticles;

    public bool IsDefaultColor { get => isDefaultColor; set => isDefaultColor = value; }
    public Color DefaultColor { get => defaultColor; }

    public void ChangeOrbColor(Color color)
    {
        foreach (var particle in orbParticles)
        {
            particle.Stop();
            particle.Clear();

            var mainModule = particle.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(color);

            particle.Play();
        }

        isDefaultColor = false;
    }

    public void ChangeOrbBackToDefaultColor()
    {
        foreach (var particle in orbParticles)
        {
            particle.Stop();
            particle.Clear();

            var mainModule = particle.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(defaultColor);

            particle.Play();
        }
        isDefaultColor = true;
    }


    public void ChangePaintColor(Color color)
    {
        foreach (var particle in paintParticles)
        {
            particle.Stop();
            particle.Clear();

            var mainModule = particle.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(color);

            particle.Play();
        }
    }

    public void PlayPaintColorAtPosition(Color color, Vector3 newPosition)
    {
        paintParent.transform.parent = null;
        paintParent.transform.position = newPosition;
        paintParent.SetActive(true);
        foreach (var particle in paintParticles)
        {
            particle.Stop();
            particle.Clear();

            var mainModule = particle.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(color);

            particle.Play();
        }
    }

    public void ChangePaintBackToDefaultColor()
    {
        foreach (var particle in paintParticles)
        {
            particle.Stop();
            particle.Clear();

            var mainModule = particle.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(defaultColor);

            particle.Play();
        }
        isDefaultColor = true;
    }

}
