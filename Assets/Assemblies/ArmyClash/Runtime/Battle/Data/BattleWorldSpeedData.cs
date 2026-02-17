using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/WorldSpeed")]
    public sealed class BattleWorldSpeedData : ComponentData
    {
        [OdinSerialize] private float _fastTimeScale = 2f;

        private readonly ReactiveProperty<float> _fastTimeScaleReactive = new ReactiveProperty<float>();

        public float FastTimeScale
        {
            get => _fastTimeScale;
            set
            {
                if (_fastTimeScale.Equals(value))
                {
                    return;
                }

                _fastTimeScale = value;
                _fastTimeScaleReactive.Value = _fastTimeScale;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<float> FastTimeScaleReactive => _fastTimeScaleReactive;

        protected override void OnFirstSetupComponent(object[] setupData = null)
        {
            _fastTimeScaleReactive.Value = _fastTimeScale;
        }
    }
}
