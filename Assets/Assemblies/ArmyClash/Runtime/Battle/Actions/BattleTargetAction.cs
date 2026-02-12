using ArmyClash.Battle.Data;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [Name("Battle/TargetAction")]
    public sealed class BattleTargetAction : BattleEntityAction
    {
        protected override void Update()
        {
            var controller = GetController();
            if (controller == null || !controller.IsRunning)
            {
                return;
            }

            if (IsDead())
            {
                return;
            }

            var targetData = Get<BattleTargetData>();
            if (targetData == null)
            {
                return;
            }

            if (targetData.Target != null && controller.IsEntityAlive(targetData.Target))
            {
                return;
            }

            var self = Self;
            if (self == null)
            {
                return;
            }

            targetData.Target = controller.FindClosestOpponent(self);
        }
    }
}
