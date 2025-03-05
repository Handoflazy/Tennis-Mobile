using UnityEngine;

namespace _Core._Scripts
{
    public class Visitor : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField]  Animator eyes;
        [SerializeField]  Renderer rend;
        [SerializeField]  GameObject hat;
        
        
        public Animator Anim => anim;
        public Animator Eyes => eyes;
        public Renderer Rend => rend;
        public GameObject Hat => hat;
        
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}