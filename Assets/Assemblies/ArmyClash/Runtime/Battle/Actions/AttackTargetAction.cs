using ArmyClash.Battle.Data;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(TargetData), typeof(StatsEntityData), typeof(BattleStatIdsData), typeof(BattleLifeData))]
    [Name("Action/AttackTarget")]
    public sealed class AttackTargetAction : CombatEntityAction
    {
        private float _cooldownRemaining;

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

            if (!stats.TryGetStatValueById(ids.AttackId, out float attack))
            {
                return;
            }

            if (!stats.TryGetStatValueById(ids.AttackSpeedId, out float attackSpeed))
            {
                return;
            }

            Vector3 current = Self.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);

            var distanceData = Get<BattleWorldAttackDistanceData>();
            float attackRange = distanceData != null ? distanceData.AttackRange : 0f;

            if (distance > attackRange)
            {
                return;
            }

            _cooldownRemaining -= Time.deltaTime;
            if (_cooldownRemaining > 0f)
            {
                return;
            }

            _cooldownRemaining = Mathf.Max(0.01f, attackSpeed);
            var targetStats = target.GetData<StatsEntityData>();
            if (!string.IsNullOrEmpty(ids.HealthId))
            {
                targetStats.AddStatValueById(ids.HealthId, -attack);
            }
        }
    }
}
