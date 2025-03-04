using UnityEngine;
using Obvious.Soap;

[CreateAssetMenu(fileName = "scriptable_event_" + nameof(Ball), menuName = "Soap/ScriptableEvents/"+ nameof(Ball))]
public class ScriptableEventBall : ScriptableEvent<Ball>
{
    
}

