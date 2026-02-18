using OdinSerializer;
using UnityEngine;

namespace ArmyClash.Battle.Config
{
    [CreateAssetMenu(menuName = "ArmyClash/Battle/WorldSpeedConfig", fileName = "BattleWorldSpeedConfig")]
    public sealed class BattleWorldSpeedConfig : ScriptableObject
    {
        [OdinSerialize]
        private float _fastTimeScale = 2f;

        public float FastTimeScale => _fastTimeScale;
    }
}
