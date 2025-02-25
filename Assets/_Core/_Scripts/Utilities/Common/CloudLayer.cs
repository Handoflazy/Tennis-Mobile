using System.Collections.Generic;
using UnityEngine;

namespace Ilumisoft.GreenMeadows
{
    public class CloudLayer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The list of prefabs used to spawn clouds")]
        private List<GameObject> cloudPrefabs = null;

        [SerializeField]
        [Tooltip("The random seed used when generating the clouds")]
        private int randomSeed = 0;

        [SerializeField, Min(0)]
        [Tooltip("The radius of the hemisphere the clouds should be spawned on")]
        private float spawnRadius = 100;

        [SerializeField,Range(0, 1)]
        [Tooltip("The lowest position (normalized) the cloud can be spawned on the hemisphere")]
        private float minLevel = 0.0f;

        [SerializeField, Range(0, 1)]
        [Tooltip("The highest position (normalized) the cloud can be spawned on the hemisphere")]
        private float maxLevel = 1.0f;

        [SerializeField, Min(0)]
        [Tooltip("The number of clouds that should be generated")]
        private int numberofClouds = 50;

        [SerializeField, Min(0)]
        [Tooltip("The minimum scale that is allowed to be applied to a cloud")]
        private float minScale = 1;

        [SerializeField, Min(0)]
        [Tooltip("The maximum scale that is allowed to be applied to a cloud")]
        private float maxScale = 1;

        [SerializeField, Min(1)]
        [Tooltip("The max horizontal stretch taht can be applied randomly to the clouds")]
        private float horizontalStretch = 1;

        [SerializeField]
        [Tooltip("How fast the clouds should rotate around the hemisphere")]
        private float rotationSpeed;

        /// <summary>
        /// The list of prefabs used to spawn clouds
        /// </summary>
        public List<GameObject> CloudPrefabs { get => cloudPrefabs; set => cloudPrefabs = value; }

        /// <summary>
        /// The random seed used when generating the clouds
        /// </summary>
        public int RandomSeed { get => randomSeed; set => randomSeed = value; }

        /// <summary>
        /// The radius of the hemisphere the clouds should be spawned on
        /// </summary>
        public float SpawnRadius { get => spawnRadius; set => spawnRadius = value; }

        /// <summary>
        /// The number of clouds that should be generated
        /// </summary>
        public int NumberofClouds { get => numberofClouds; set => numberofClouds = value; }

        /// <summary>
        /// The max horizontal stretch taht can be applied randomly to the clouds
        /// </summary>
        public float HorizontalStretch { get => horizontalStretch; set => horizontalStretch = value; }

        /// <summary>
        /// The minimum scale that is allowed to be applied to a cloud
        /// </summary>
        public float MinScale { get => minScale; set => minScale = value; }

        /// <summary>
        /// The maximum scale that is allowed to be applied to a cloud
        /// </summary>
        public float MaxScale { get => maxScale; set => maxScale = value; }

        /// <summary>
        /// The lowest position (normalized) the cloud can be spawned on the hemisphere
        /// </summary>
        public float MinLevel { get => minLevel; set => minLevel = value; }

        /// <summary>
        /// The highest position (normalized) the cloud can be spawned on the hemisphere
        /// </summary>
        public float MaxLevel { get => maxLevel; set => maxLevel = value; }

        void Update()
        {
            RotateClouds();
        }

        private void RotateClouds()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}