using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using ArmyClash.Battle.Data;

namespace ArmyClash.Battle.Services
{
    public sealed class BattleStateService
    {
        private readonly ReactiveProperty<SimulationState> _simulationState = new ReactiveProperty<SimulationState>();
        private readonly BattleTeamRoster _roster;

        public BattleStateService(BattleTeamRoster roster)
        {
            _roster = roster;
        }

        public SimulationState SimulationState => _simulationState.Value;
        public IReadOnlyReactiveProperty<SimulationState> SimulationStateReactive => _simulationState;

        public void StartBattle()
        {
            if (_roster.LeftCount == 0 || _roster.RightCount == 0)
            {
                return;
            }

            _simulationState.Value = SimulationState.Running;
        }

        public void UnregisterEntity(EntityMonoBehaviour entity)
        {
            _roster.Unregister(entity);

            if (_roster.LeftCount == 0 || _roster.RightCount == 0)
            {
                FinishBattle();
            }
        }

        public void FinishBattle()
        {
            _simulationState.Value = SimulationState.Finished;
        }

        public void SetIdle()
        {
            _simulationState.Value = SimulationState.Idle;
        }
    }
}
