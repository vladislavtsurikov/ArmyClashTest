using System;
using ArmyClash.Battle.Actions;

namespace ArmyClash.Battle
{
    public sealed class BattleHeroEntity : BattleEntity
    {
        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(FindClosestOpponentTargetAction),
                typeof(MoveToTargetAction),
                typeof(AttackTargetAction),
                typeof(RegenHealthAction),
                typeof(HealthToDeathAction),
                typeof(HandleDeathAction)
            };
        }
    }
}
