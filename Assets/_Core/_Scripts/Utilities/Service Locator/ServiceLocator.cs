using System.Linq;
using Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _global; // Global instance of the ServiceLocator
        private static Dictionary<Scene, ServiceLocator> _sceneContainers; // Dictionary to hold ServiceLocators for each scene
        private static List<GameObject> _tmpSceneGameObject; // Temporary list to hold root game objects in a scene

        private readonly ServiceManager services = new(); // Instance of ServiceManager to manage services

        const string k_globalSeviceLocatorName = "ServiceLocator [Global]"; // Name for the global ServiceLocator GameObject
        const string k_sceneSeviceLocatorName = "ServiceLocator [Scene]"; // Name for the global ServiceLocator GameObject

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad) {
            if(_global == this) {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: This ServiceLocator is already configured as global",this);
            }else if(_global != null) {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global",this);
                
            } else {
                _global = this;
                if(dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
        }
        
        internal void ConfigureAsScene() {
            Scene scene = gameObject.scene;
            if(_sceneContainers.ContainsKey(scene)) {
                Debug.LogWarning("ServiceLocator.ConfigureAsScene: This ServiceLocator is already configured for this scene",this);
            } else {
                _sceneContainers.Add(scene, this);
            }
        }
        
        
        #region Level Locator
        // Property to get the global ServiceLocator instance
        public static ServiceLocator Global {
            get {
                if(_global != null) return _global;
                if(FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found) {
                    found.BootstrapOnDemand();
                    return _global;
                }

                var container = new GameObject(k_globalSeviceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();

                return _global;
            }
        }

        // Method to get the ServiceLocator for a given MonoBehaviour
        public static ServiceLocator For(MonoBehaviour mb) {
            return mb.GetComponentInChildren<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global;
        }

        // Method to get the ServiceLocator for the scene of a given MonoBehaviour
        public static ServiceLocator ForSceneOf(MonoBehaviour mb) {
            Scene scene = mb.gameObject.scene;
            if(_sceneContainers.TryGetValue(scene, out ServiceLocator container) && container != mb) {
                return container;
            }
            _tmpSceneGameObject.Clear();
            scene.GetRootGameObjects(_tmpSceneGameObject);
            foreach (GameObject go in _tmpSceneGameObject.Where(go=>go.TryGetComponent<ServiceLocatorSceneBootstrapper>(out _))) {
                if(go.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container!=mb)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return _global;
        }
        #endregion

        // Method to register a service of type T
        public ServiceLocator Register<T>(T service) {
            services.Register(service);
            return this;
        }

        // Method to register a service with a specified type
        public ServiceLocator Register(Type type, object service) {
            services.Register(type, service);
            return this;
        }

        // Method to get a registered service of type T
        public ServiceLocator Get<T>(out T service) where T: class {
            if(TryGetService(out service)) return this;

            if(TryGetNextInHierarchy(out ServiceLocator container)) {
                container.Get(out service);
                return this;
            }
            throw new InvalidOperationException($"ServiceLocator.Get: Service of type {typeof(T).FullName} is not registered");
        }

        #region Helper Methods
        // Method to try to get a registered service of type T
        bool TryGetService<T>(out T service) where T: class {
            return services.TryGet(out service);
        }

        // Method to try to get the next ServiceLocator in the hierarchy
        bool TryGetNextInHierarchy(out ServiceLocator container){
            if(this == _global) {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull()??ForSceneOf(this);
            return container != null;
        }
        #endregion

        private void OnDestroy() {
            if(this == _global) {
                _global = null;
            } else if(_sceneContainers.ContainsValue(this)) {
                _sceneContainers.Remove(gameObject.scene);
            }
            services.Clear();
        }
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatic() {
            _global = null;
            _sceneContainers = new Dictionary<Scene, ServiceLocator>();
            _tmpSceneGameObject = new List<GameObject>();
        }
#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/Add Global")]
        static void AddGlobal() {
            var go = new GameObject(k_globalSeviceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add Scene")]
        static void AddScene() {
            var go = new GameObject(k_sceneSeviceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
        }
#endif
    }
}