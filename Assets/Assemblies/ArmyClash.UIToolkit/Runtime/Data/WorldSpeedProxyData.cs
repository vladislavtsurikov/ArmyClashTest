using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Data
{
    [Name("UI/WorldSpeedProxy")]
    public sealed class WorldSpeedProxyData : ComponentData
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
