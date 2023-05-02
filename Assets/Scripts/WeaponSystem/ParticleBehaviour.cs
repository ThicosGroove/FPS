using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    [SerializeField]private GameObject _particleHitMarker;

    private ParticleSystem _particleSystemBullet;
    private List<ParticleCollisionEvent> _collisionEvents;

    public float finalDamage;

    private void Start()
    {
        _particleSystemBullet = GetComponent<ParticleSystem>();
        _collisionEvents = new List<ParticleCollisionEvent>();
    }


    public void ParticlePlay(float sizeMulti, float damageMulti)
    {
        finalDamage = damageMulti;

        if (_particleSystemBullet == null)
        {
            Debug.LogWarning("Particle System Is Null");
            return;
        }

        
        _particleSystemBullet.Play();
    }


    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _particleSystemBullet.GetCollisionEvents(other, _collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            Debug.LogWarning($"ACERTOU {other.name}");
            var newHitMarker = Instantiate(_particleHitMarker, _collisionEvents[i].intersection, Quaternion.LookRotation(_collisionEvents[i].normal));
            //Criar Pool para essa nova particula.
            Destroy(newHitMarker, 1f);


            if (rb)
            {
                Vector3 pos = _collisionEvents[i].intersection;
                Vector3 force = _collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            i++;
        }
    }
}
