using Platformer.Factory;
using UnityEngine;

namespace _Core._Scripts
{
    [CreateAssetMenu(menuName = "Factory/VisitorFactory", fileName = "VisitorFactory")]
    public class VisitorFactory : FactorySO<Visitor>
    {
        [SerializeField] Visitor visitorPrefab;
        
        public Visitor Prefab
        {
            get => visitorPrefab;
            set => visitorPrefab = value;
        }
        public override Visitor Create() {
            return Instantiate(visitorPrefab);
        }
    }
}