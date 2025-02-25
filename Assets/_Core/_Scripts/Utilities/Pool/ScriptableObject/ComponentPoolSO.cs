using System.Collections.Generic;
using System.Linq;
using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool
{
    
    /// <summary>
    /// Implements a Pool for Component types.
    /// </summary>
    /// <typeparam name="T">Specifies the component to pool.</typeparam
    public abstract class ComponentPoolSO<T>: PoolSO<T> where T: Component
    {
        
        public override IFactory<T> Factory { get; set; }

        private Transform poolRoot;
        private Transform PoolRoot
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }

                if (poolRoot != null) return poolRoot;
                poolRoot = new GameObject(name).transform;
                poolRoot.SetParent(parent);
                return poolRoot;
            }
        }

        private Transform parent;

        public void SetParent(Transform t)
        {
            parent = t;
            PoolRoot.SetParent(parent);
        }
        public override T Request()
        {
            T member =  base.Request();
            member.gameObject.SetActive(true);
            return member;
        }

        public override void Return(T member)
        {
            member.transform.SetParent(PoolRoot);
            member.gameObject.SetActive(false);
            base.Return(member);
        }
        
        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot);
            return newMember;
        }
        
        public  IEnumerable<T> GetAvailable()
        {
            var getAvailable = new List<T>(available);
            return getAvailable;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            if(poolRoot)
                DestroyImmediate(poolRoot.gameObject);
#else   
            Destroy(poolRoot.gameObject);
#endif  
        }
    }
}