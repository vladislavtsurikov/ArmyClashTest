using UnityEngine;

namespace ArmyClash.UnitTraits
{
    [CreateAssetMenu(menuName = "Unit/Traits/Shape", fileName = "ShapeModifier")]
    public sealed class ShapeModifier : ModifierConfig
    {
        [SerializeField] private GameObject _prefab;

        public GameObject Prefab => _prefab;
    }
}
