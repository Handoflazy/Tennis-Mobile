using Platformer.Factory;
using UnityEngine;

namespace Platformer.Factory
{
    [CreateAssetMenu(fileName = "New Particle Factory", menuName = "Factory/Particle Factory")]
    public class ParticleFactorySO : FactorySO<ParticleSystem>
    {
        [SerializeField]
        private ParticleSystem prefab = default;

        public ParticleSystem Prefab
        {
            get => prefab;
            set => prefab = value;
        }

        public override ParticleSystem Create()
        {
            return Instantiate(prefab);
        }
    }
}