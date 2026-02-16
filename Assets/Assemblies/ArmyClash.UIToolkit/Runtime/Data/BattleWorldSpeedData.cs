using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Data
{
    [Name("Battle/WorldSpeed")]
    public sealed class BattleWorldSpeedData : ComponentData
    {
        [OdinSerialize] private float _fastTimeScale = 2f;

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
                MarkDirty();
            }
        }
    }
}
