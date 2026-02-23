using UnityEngine;

namespace ArmyClash.Battle.Config
{
    [CreateAssetMenu(menuName = "Configs/Battle/WorldSpeedConfig", fileName = "BattleWorldSpeedConfig")]
    public sealed class BattleWorldSpeedConfig : ScriptableObject
    {
        [SerializeField]
        private float _fastTimeScale = 2f;

        public float FastTimeScale => _fastTimeScale;
    }
}
