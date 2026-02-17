using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/WorldAutoRandomize")]
    public sealed class BattleWorldAutoRandomizeData : ComponentData
    {
        [OdinSerialize] private bool _autoRandomizeOnAwake = true;

        private readonly ReactiveProperty<bool> _autoRandomizeReactive = new ReactiveProperty<bool>();

        public bool AutoRandomizeOnAwake
        {
            get => _autoRandomizeOnAwake;
            set
            {
                if (_autoRandomizeOnAwake == value)
                {
                    return;
                }

                _autoRandomizeOnAwake = value;
                _autoRandomizeReactive.Value = _autoRandomizeOnAwake;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<bool> AutoRandomizeReactive => _autoRandomizeReactive;

        protected override void OnFirstSetupComponent(object[] setupData = null)
        {
            _autoRandomizeReactive.Value = _autoRandomizeOnAwake;
        }
    }
}
