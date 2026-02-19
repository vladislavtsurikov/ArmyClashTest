using System;
using ArmyClash.Battle.Actions;
using VladislavTsurikov.StateMachine.Runtime.Actions;

namespace ArmyClash.Battle
{
    public sealed class BattleHeroEntity : BattleEntity
    {
        protected override Type[] ActionTypesToCreate() =>
            new[] { typeof(StateMachineAction), typeof(HandleDeathAction), typeof(RegenHealthAction) };
    }
}
