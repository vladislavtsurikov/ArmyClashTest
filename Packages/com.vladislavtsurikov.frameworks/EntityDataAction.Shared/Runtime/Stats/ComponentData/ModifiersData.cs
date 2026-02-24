using OdinSerializer;
using UniRx;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/ModifiersData")]
    public sealed class ModifiersData : ComponentData
    {
        [OdinSerialize]
        private readonly ReactiveCollection<ModifierStatEffect> _effects = new();

        public ReactiveCollection<ModifierStatEffect> Effects => _effects;

        public void Clear()
        {
            _effects.Clear();
            MarkDirty();
        }

        public void Add(ModifierStatEffect effect)
        {
            _effects.Add(effect);
            MarkDirty();
        }

        public bool Remove(ModifierStatEffect effect)
        {
            bool removed = _effects.Remove(effect);
            MarkDirty();
            return removed;
        }
    }
}
