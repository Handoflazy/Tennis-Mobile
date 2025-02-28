using UnityEngine;

namespace UnityServiceLocator
{
    [AddComponentMenu("Service Locator/Service Locator Global Bootstrapper")]
    public class ServiceLocatorGlobalBootstrapper : Bootstrapper {
        [SerializeField] bool dontDestroyOnLoad = true;
        protected override void Bootstrap() {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}