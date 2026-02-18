using System;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Actions;
using ArmyClash.Battle.States;
using VladislavTsurikov.StateMachine.Runtime.Actions;
using VladislavTsurikov.StateMachine.Runtime.Data;

namespace ArmyClash.Battle
{
    public abstract class BattleEntity : EntityMonoBehaviour
    {
        protected override Type[] ComponentDataTypesToCreate()
        {
            return new[]
            {
                typeof(StatsEntityData),
                typeof(StatIdsData),
                typeof(TeamData),
                typeof(TargetData),
                typeof(LifeData),
                typeof(AttackDistanceData),
                typeof(StateMachineData)
            };
        }

        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(StateMachineAction),
                typeof(HandleDeathAction)
            };
        }

        protected override void OnAfterCreateDataAndActions()
        {
            var stateMachine = GetData<StateMachineData>();
            if (stateMachine == null)
            {
                return;
            }

            var stack = stateMachine.StateStack;
            if (stack == null || stack.ElementList.Count > 0)
            {
                return;
            }

            stack.CreateIfMissingType(new[]
            {
                typeof(DeadState),
                typeof(IdleState),
                typeof(FindTargetState),
                typeof(MoveToTargetState),
                typeof(AttackTargetState)
            });
        }
    }
}
