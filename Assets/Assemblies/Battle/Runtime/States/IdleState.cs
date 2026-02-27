using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;
using Zenject;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/Idle")]
    public sealed class IdleState : State
    {
        [Inject]
        private BattleStateService _state;

        protected override bool Conditional() =>
            _state.SimulationStateReactive.Value != SimulationState.Running;
    }
}
