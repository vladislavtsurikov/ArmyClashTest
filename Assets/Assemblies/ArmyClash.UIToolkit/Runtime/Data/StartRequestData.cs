using OdinSerializer;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Data
{
    [Name("UI/ArmyClash/StartRequestData")]
    public sealed class StartRequestData : ComponentData
    {
        [OdinSerialize]
        private int _requestId;

        public int RequestId
        {
            get => _requestId;
            set
            {
                if (_requestId == value)
                {
                    return;
                }

                _requestId = value;
                MarkDirty();
            }
        }

        public void Request()
        {
            RequestId++;
        }
    }
}
