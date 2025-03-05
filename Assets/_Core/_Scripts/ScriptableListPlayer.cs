using UnityEngine;
using Obvious.Soap;

[CreateAssetMenu(fileName = "scriptable_list_" + nameof(Player), menuName = "Soap/ScriptableLists/"+ nameof(Player))]
public class ScriptableListPlayer : ScriptableList<Player>
{
    
}
