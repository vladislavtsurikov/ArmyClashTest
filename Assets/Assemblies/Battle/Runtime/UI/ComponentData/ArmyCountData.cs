using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.UI.Data
{
    [Name("UI/ArmyClash/ArmyCountData")]
    public sealed class ArmyCountData : ComponentData
    {
        [OdinSerialize]
        private int _leftCount;

        [OdinSerialize]
        private int _rightCount;

        public int LeftCount
        {
            get => _leftCount;
            set
            {
                if (_leftCount == value)
                {
                    return;
                }

                _leftCount = value;
                MarkDirty();
            }
        }

        public int RightCount
        {
            get => _rightCount;
            set
            {
                if (_rightCount == value)
                {
                    return;
                }

                _rightCount = value;
                MarkDirty();
            }
        }
    }
}
