using UnityEditor;
using UnityEngine;

namespace Utilities.Tools
{
    public class ClickToPlace : MonoBehaviour
    {
        [Tooltip("Vertical offset above the clicked point. Useful to avoid spawn points to be directly ON the geometry which might cause issues.")]
        [SerializeField] private float _verticalOffset = 0.1f;
        
        private Vector3 targetPosition;
        public bool IsTargeting { get; private set; }
        private void OnDrawGizmos()
        {
            if (IsTargeting)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(targetPosition, Vector3.one * 0.3f);
            }
        }
        public void BeginTargeting()
        {
            IsTargeting = true;
            targetPosition = transform.position;
        }
        public void UpdateTargeting(Vector3 spawnPosition)
        {
            targetPosition = spawnPosition + Vector3.up * _verticalOffset;
        }
        public void EndTargeting()
        {
            IsTargeting = false;
#if UNITY_EDITOR
            Undo.RecordObject(transform, $"{gameObject.name} moved by ClickToPlaceHelper");
#endif
            transform.position = targetPosition;
        }
    }
}