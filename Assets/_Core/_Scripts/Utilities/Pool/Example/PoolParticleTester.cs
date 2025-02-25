using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Platformer.Factory
{
    public class PoolParticleTester: MonoBehaviour
    {
        [SerializeField] private ParticlePoolSO pool = default;

        private IEnumerator Start()
        {
            List<ParticleSystem> particles = pool.Request(10) as List<ParticleSystem>;
            foreach (var particle in particles)
            {
                particle.transform.position = Random.insideUnitSphere * 5f;
                particle.Play();
                yield return new WaitForSeconds(5f);
            }
            pool.Return(particles);
        }
    }
}