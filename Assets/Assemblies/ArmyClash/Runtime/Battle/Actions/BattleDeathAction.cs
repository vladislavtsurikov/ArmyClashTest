using ArmyClash.Battle.Data;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [Name("Battle/DeathAction")]
    public sealed class BattleDeathAction : BattleEntityAction
    {
        protected override void Update()
        {
            var controller = GetController();
            if (controller == null || !controller.IsRunning)
            {
                return;
            }

            var life = Get<BattleLifeData>();
            if (life == null || life.IsDead)
            {
                return;
            }

            var stats = GetStats();
            if (stats == null)
            {
                return;
            }

            if (!controller.TryGetHealth(stats, out float health))
            {
                return;
            }

            if (health > 0f)
            {
                return;
            }

            life.IsDead = true;

            var self = Self;
            if (self == null)
            {
                return;
            }

            controller.HandleEntityDeath(self);
        }
    }
}
