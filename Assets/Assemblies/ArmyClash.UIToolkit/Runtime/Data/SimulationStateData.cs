using OdinSerializer;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Data
{
    public enum SimulationState
    {
        Idle,
        Running,
        Finished
    }

    [Name("UI/ArmyClash/SimulationStateData")]
    public sealed class SimulationStateData : ComponentData
    {
        [OdinSerialize]
        private SimulationState _state;

        public SimulationState State
        {
            get => _state;
            set
            {
                if (_state == value)
                {
                    return;
                }

                _state = value;
                MarkDirty();
            }
        }
    }
}
