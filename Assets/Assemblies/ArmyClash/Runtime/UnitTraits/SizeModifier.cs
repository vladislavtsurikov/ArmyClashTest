using UnityEngine;

namespace ArmyClash.UnitTraits
{
    [CreateAssetMenu(menuName = "Unit/Traits/Size", fileName = "SizeModifier")]
    public sealed class SizeModifier : ModifierConfig
    {
        [SerializeField] private Vector3 _scale = Vector3.one;

        public Vector3 Scale => _scale;
    }
}
