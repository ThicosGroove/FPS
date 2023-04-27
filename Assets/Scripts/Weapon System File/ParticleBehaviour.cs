using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private List<ParticleCollisionEvent> _colEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        var events = particle.GetCollisionEvents(other, _colEvents);

        Debug.LogWarning("ACERTOU ALGO");

        for (int i = 0; i < events; i++)
        {

        }
    }
}
