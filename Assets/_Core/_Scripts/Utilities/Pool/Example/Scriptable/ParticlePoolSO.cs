using Platformer.Factory;
using Platformer.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Factory
{
    [CreateAssetMenu(fileName = "New Particle Pool", menuName = "Pool/Particle Pool")]
    public class ParticlePoolSO: ComponentPoolSO<ParticleSystem>
    {
        [SerializeField]
        private ParticleFactorySO factory;

        public override  IFactory<ParticleSystem> Factory
        {
            get { return factory; }
            set { factory = value as ParticleFactorySO; }
        }
    }
}