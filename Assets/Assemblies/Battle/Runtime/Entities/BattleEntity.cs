using System;
using ArmyClash.Battle.Actions;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.States;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.StateMachine.Runtime.Actions;
using VladislavTsurikov.StateMachine.Runtime.Data;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle
{
    public class BattleEntity : EntityMonoBehaviour
    {
        protected override Type[] ComponentDataTypesToCreate() =>
            new[]
            {
                typeof(StatsEntityData), typeof(ModifiersData), typeof(TeamData), typeof(TargetData), typeof(LifeData),
                typeof(AttackDistanceData), typeof(StateMachineData)
            };

        protected override Type[] ActionTypesToCreate() =>
            new[]
            {
                typeof(ApplyModifierStatEffectAction),
                typeof(ApplyColorModifierAction),
                typeof(ApplySizeModifierAction),
                typeof(ApplyShapeModifierAction),
                typeof(SelectRandomModifierEffectAction),
                typeof(SelectRandomModifierEffectAction),
                typeof(SelectRandomModifierEffectAction),
                typeof(StateMachineAction),
                typeof(HandleDeathAction)
            };

        protected override void OnAfterCreateDataAndActions()
        {
            StateMachineData stateMachine = GetData<StateMachineData>();

            NodeStackOnlyDifferentTypes<State> stack = stateMachine.StateStack;
            if (stack.ElementList.Count == 0)
            {
                stack.CreateIfMissingType(new[]
                {
                    typeof(DeadState), typeof(IdleState), typeof(FindTargetState), typeof(MoveToTargetState),
                    typeof(AttackTargetState)
                });
            }
        }
    }
}
