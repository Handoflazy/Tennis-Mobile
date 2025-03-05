using UnityEngine;
using Obvious.Soap;

[CreateAssetMenu(fileName = "scriptable_event_" + nameof(Player), menuName = "Soap/ScriptableEvents/"+ nameof(Player))]
public class ScriptableEventPlayer : ScriptableEvent<Player>
{
    
}

