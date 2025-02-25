using UnityEngine;

namespace Utilities.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Empty Channel")]
    
    public class EmptyEventChannel : EventChannel<Empty>
    {
        
    }
    public readonly struct Empty{}
}