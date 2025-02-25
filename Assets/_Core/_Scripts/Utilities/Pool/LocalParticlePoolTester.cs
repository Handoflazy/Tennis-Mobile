using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Factory
{
    public class LocalParticlePoolTester : MonoBehaviour
    {
        [SerializeField] private ParticleSystem prefab = default;
        [SerializeField] private int InitialPoolSize = 5;

        private ParticlePoolSO pool;
        private ParticleFactorySO factory;

        private void Start()
        {
            factory = ScriptableObject.CreateInstance<ParticleFactorySO>();
            factory.Prefab = prefab;
            pool = ScriptableObject.CreateInstance<ParticlePoolSO>();
            pool.name = gameObject.name;
            pool.Factory = factory;
            pool.SetParent(this.transform);
            pool.Prewarm(InitialPoolSize);
            List<ParticleSystem> particles = pool.Request(2) as List<ParticleSystem>;
            foreach (var particle in particles)
            {
                particle.transform.position = Random.insideUnitSphere * 5f;
                particle.Play();
            }
            foreach (ParticleSystem particle in particles)
            {
                StartCoroutine(DoParticleBehaviour(particle));
            }
        }
        private IEnumerator DoParticleBehaviour(ParticleSystem particle)
        {
            particle.transform.position = Random.insideUnitSphere * 5f;
            particle.Play();
            yield return new WaitForSeconds(particle.main.duration);
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            yield return new WaitUntil(() => particle.particleCount == 0);
            pool.Return(particle);
        }
    }
}