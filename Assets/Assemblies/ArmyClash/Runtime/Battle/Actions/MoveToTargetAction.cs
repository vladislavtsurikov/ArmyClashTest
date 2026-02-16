using ArmyClash.Battle.Data;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(TargetData), typeof(StatsEntityData), typeof(BattleStatIdsData), typeof(BattleLifeData))]
    [Name("Action/MoveToTarget")]
    public sealed class MoveToTargetAction : CombatEntityAction
    {
        protected override void Update()
        {
            var stateAction = GetStateAction();
            if (!stateAction.IsRunning)
            {
                return;
            }

            if (IsDead())
            {
                return;
            }

            var targetData = Get<TargetData>();
            if (targetData.Target == null)
            {
                return;
            }

            var target = targetData.Target;
            var rosterAction = GetRosterAction();
            if (!rosterAction.IsEntityAlive(target))
            {
                return;
            }

            var stats = GetStats();
            var ids = GetStatIds();

            if (!stats.TryGetStatValueById(ids.SpeedId, out float speed))
            {
                return;
            }

            var distanceData = Get<BattleWorldAttackDistanceData>();
            float attackRange = distanceData != null ? distanceData.AttackRange : 0f;
            float stopDistance = distanceData != null ? distanceData.StopDistance : 0f;

            Vector3 current = Self.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 direction = targetPosition - current;
            float distance = direction.magnitude;

            if (distance <= Mathf.Max(0f, attackRange - stopDistance))
            {
                return;
            }

            Vector3 next = Vector3.MoveTowards(current, targetPosition, speed * Time.deltaTime);
            Self.transform.position = next;
        }
    }
}
