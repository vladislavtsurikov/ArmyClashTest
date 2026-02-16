using System;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using ArmyClash.Battle.Actions;
using ArmyClash.Battle.Data;

namespace ArmyClash.Battle
{
    public abstract class BattleEntity : EntityMonoBehaviour
    {
        protected override Type[] ComponentDataTypesToCreate()
        {
            return new[]
            {
                typeof(StatsEntityData),
                typeof(BattleStatIdsData),
                typeof(BattleContextData),
                typeof(BattleTeamData),
                typeof(TargetData),
                typeof(BattleLifeData)
            };
        }

        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(FindClosestOpponentTargetAction),
                typeof(TargetValidationAction),
                typeof(MoveToTargetAction),
                typeof(AttackTargetAction),
                typeof(HealthToDeathAction),
                typeof(HandleDeathAction)
            };
        }
    }
}
