using UnityEngine;
using Utilities.Extensions;

namespace UnityServiceLocator
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper: MonoBehaviour
    {
        private ServiceLocator container;
        internal ServiceLocator Container => container.OrNull()??(container = GetComponent<ServiceLocator>()) ;
             
        bool hasBeenBootstrapped;

        private void Awake() {
            BootstrapOnDemand();
        }

        public void BootstrapOnDemand() {
            if(hasBeenBootstrapped) return;
            hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}