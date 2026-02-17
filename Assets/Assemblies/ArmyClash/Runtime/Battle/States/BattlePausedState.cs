using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/Paused")]
    public sealed class BattlePausedState : State
    {
        protected override void Conditional()
        {
            var worldState = GetAction<BattleWorldStateAction>();
            BindEligibility(worldState.IsRunningReactive.Select(isRunning => !isRunning));
        }
    }
}
