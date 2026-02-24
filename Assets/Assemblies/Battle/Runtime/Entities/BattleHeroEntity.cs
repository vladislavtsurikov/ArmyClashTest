using System;
using ArmyClash.Battle.Actions;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.StateMachine.Runtime.Actions;

namespace ArmyClash.Battle
{
    public sealed class BattleHeroEntity : BattleEntity
    {
        protected override Type[] ActionTypesToCreate() =>
            new[]
            {
                typeof(SelectRandomModifierEffectAction),
                typeof(SelectRandomModifierEffectAction),
                typeof(SelectRandomModifierEffectAction),
                typeof(ApplyModifierStatEffectAction),
                typeof(ApplyColorModifierAction),
                typeof(ApplySizeModifierAction),
                typeof(ApplyShapeModifierAction),
                typeof(StateMachineAction),
                typeof(HandleDeathAction),
                typeof(RegenHealthAction)
            };
    }
}
