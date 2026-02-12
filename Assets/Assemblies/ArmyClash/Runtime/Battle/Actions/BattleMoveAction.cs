using ArmyClash.Battle.Data;
using UnityEngine;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [Name("Battle/MoveAction")]
    public sealed class BattleMoveAction : BattleEntityAction
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

            if (!controller.TryGetSpeed(stats, out float speed))
            {
                return;
            }

            float attackRange = controller.AttackRange;
            float stopDistance = controller.StopDistance;

            Vector3 current = self.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 direction = targetPosition - current;
            float distance = direction.magnitude;

            if (distance <= Mathf.Max(0f, attackRange - stopDistance))
            {
                return;
            }

            Vector3 next = Vector3.MoveTowards(current, targetPosition, speed * Time.deltaTime);
            self.transform.position = next;
        }
    }
}
