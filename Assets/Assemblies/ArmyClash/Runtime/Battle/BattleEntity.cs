using System;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using ArmyClash.Battle.Actions;
using ArmyClash.Battle.Data;

namespace ArmyClash.Battle
{
    public sealed class BattleEntity : EntityMonoBehaviour
    {
        protected override Type[] ComponentDataTypesToCreate()
        {
            return new[]
            {
                typeof(StatsEntityData),
                typeof(BattleContextData),
                typeof(BattleTeamData),
                typeof(BattleTargetData),
                typeof(BattleLifeData)
            };
        }

        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(BattleTargetAction),
                typeof(BattleMoveAction),
                typeof(BattleAttackAction),
                typeof(BattleRegenAction),
                typeof(BattleDeathAction)
            };
        }
    }
}
