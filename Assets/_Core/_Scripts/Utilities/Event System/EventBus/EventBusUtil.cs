using System;
using System.Collections.Generic;
using System.Reflection;
using Utilities;
using UnityEditor;
using UnityEngine;

namespace Utilities.Event_System.EventBus
{
    public static class EventBusUtil
    {
        public static IReadOnlyList<Type> EventTypes { get; set; }
        public static IReadOnlyList<Type> EventBusTypes { get; set; }
        
        #if UNITY_EDITOR 
            public static PlayModeStateChange PlayerModeState { get; set; }
            [InitializeOnLoadMethod]
            public static void InitializeEditor()
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            private static void OnPlayModeStateChanged(PlayModeStateChange state)
            {
                PlayerModeState =  state;
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    ClearAllBus();
                }
            }
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefineAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBuses();
        }

        private static List<Type> InitializeAllBuses()
        {
            List<Type> eventBusTypes = new List<Type>();
            var typedef = typeof(EventBus<>);
            foreach (var eventType in EventTypes)
            {
                var busType = typedef.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
                Debug.Log($"Initialized EventBus<{eventType.Name}");
                
            }

            return eventBusTypes;
        }

        public static void ClearAllBus()
        {
            Debug.Log("Clear all Bus");
            for (int i = 0; i < EventBusTypes.Count; i++)
            {
                var bustType = EventBusTypes[i];
                var clearMethod = bustType.GetMethod("Clear", BindingFlags.Static|BindingFlags.NonPublic);
                clearMethod.Invoke(null, null);
            }
        }
    }
}