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
    [AddComponentMenu("Service Locator/Service Locator Global Bootstrapper")]
    public class ServiceLocatorGlobalBootstrapper : Bootstrapper {
        [SerializeField] bool dontDestroyOnLoad = true;
        protected override void Bootstrap() {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
    [AddComponentMenu("Service Locator/Service Locator Scene Bootstrapper")]
    public class ServiceLocatorSceneBootstrapper : Bootstrapper {
        protected override void Bootstrap() {
            Container.ConfigureAsScene();
        }
    }
}