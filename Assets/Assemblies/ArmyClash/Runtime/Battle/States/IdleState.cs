using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;
using Zenject;
using ArmyClash.Battle.Services;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/Idle")]
    public sealed class IdleState : State
    {
        [Inject]
        private BattleStateService _state;

        protected override void Conditional()
        {
            BindEligibility(_state.SimulationStateReactive.Select(state => state != SimulationState.Running));
        }
    }
}
