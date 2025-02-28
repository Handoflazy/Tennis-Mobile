using UnityEngine;

namespace UnityServiceLocator
{
    [AddComponentMenu("Service Locator/Service Locator Scene Bootstrapper")]
    public class ServiceLocatorSceneBootstrapper : Bootstrapper {
        protected override void Bootstrap() {
            Container.ConfigureAsScene();
        }
    }
}