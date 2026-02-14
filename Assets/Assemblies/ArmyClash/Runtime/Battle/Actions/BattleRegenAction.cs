using UnityEngine;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [Name("Battle/RegenAction")]
    public sealed class BattleRegenAction : BattleEntityAction
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

            var self = Self;
            if (self == null)
            {
                return;
            }

            var stats = GetStats();
            if (stats == null)
            {
                return;
            }

            if (!controller.TryGetRegen(stats, out float regen))
            {
                return;
            }

            if (regen <= 0f)
            {
                return;
            }

            controller.ApplyHealing(self, regen * Time.deltaTime);
        }
    }
}
