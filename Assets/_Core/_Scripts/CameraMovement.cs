using UnityEngine;

namespace _Core._Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform player;
        public Transform opponent;
        public Transform ball;
        public float smoothSpeed = 0.125f;
        public Vector3 offset;

        public void Zoom(bool zoom) {
            
        }
    }
}