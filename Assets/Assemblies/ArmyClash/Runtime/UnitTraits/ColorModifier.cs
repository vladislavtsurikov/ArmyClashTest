using UnityEngine;

namespace ArmyClash.UnitTraits
{
    [CreateAssetMenu(menuName = "Unit/Traits/Color", fileName = "ColorModifier")]
    public sealed class ColorModifier : ModifierConfig
    {
        [SerializeField] private Color _color = Color.white;

        public Color Color => _color;
    }
}
