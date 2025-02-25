using UnityEngine;

namespace Utilities.EventChannel
{
    [CreateAssetMenu(menuName = "Events/UI/Player Stats ChannelSO")]
    public class PlayerStatsChannelSO : EventChannelSO<PlayerStats>
    {
        
    }
    public struct PlayerStats
    {
        public int Health;
        public int Damage;
        public int Defense;
    }
}