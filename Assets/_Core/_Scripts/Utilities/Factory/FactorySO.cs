using UnityEngine;

namespace Platformer.Factory
{

    public abstract class FactorySO<T> : ScriptableObject, IFactory<T>
    {
        public abstract T Create();
    }
}