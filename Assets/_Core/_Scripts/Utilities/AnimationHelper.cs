using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class AnimationHelper: MonoBehaviour
    {
        public UnityEvent AnimationEvent;
        public UnityEvent PickUpEvent;
        public UnityEvent DissolveEvent;
        public void RaiseConsumeAttack() => AnimationEvent?.Invoke();
        public void RaisePickUpEvent() => PickUpEvent?.Invoke();
        public void RaiseDissolveEvent() => DissolveEvent?.Invoke();
        
        
    }
}