using ArmyClash.Battle.Data;
using UnityEngine;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [Name("Battle/AttackAction")]
    public sealed class BattleAttackAction : BattleEntityAction
    {
        private float _cooldownRemaining;

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
            if (targetData == null || targetData.Target == null)
            {
                return;
            }

            var target = targetData.Target;
            if (!controller.IsEntityAlive(target))
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

            if (!controller.TryGetAttack(stats, out float attack))
            {
                return;
            }

            if (!controller.TryGetAttackSpeed(stats, out float attackSpeed))
            {
                return;
            }

            Vector3 current = self.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);

            if (distance > controller.AttackRange)
            {
                return;
            }

            _cooldownRemaining -= Time.deltaTime;
            if (_cooldownRemaining > 0f)
            {
                return;
            }

            _cooldownRemaining = Mathf.Max(0.01f, attackSpeed);
            controller.ApplyDamage(target, attack);
        }
    }
}
