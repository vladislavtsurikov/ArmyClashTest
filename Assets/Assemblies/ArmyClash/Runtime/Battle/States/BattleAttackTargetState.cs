using ArmyClash.Battle.Data;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/AttackTarget")]
    public sealed class BattleAttackTargetState : State
    {
        private float _cooldownRemaining;

        protected override void Conditional()
        {
            BindEligibility(() => CanAttack(Entity));
        }

        public override void Update(Entity entity, float deltaTime)
        {
            if (!IsRunning(entity) || IsDead(entity))
            {
                return;
            }

            if (!TryGetValidTarget(entity, out BattleEntity target))
            {
                return;
            }

            var battleEntity = GetBattleEntity(entity);
            var ids = entity.GetData<BattleStatIdsData>();
            if (!TryGetStatValue(entity, ids.AttackId, out float attack))
            {
                return;
            }

            if (!TryGetStatValue(entity, ids.AttackSpeedId, out float attackSpeed))
            {
                return;
            }

            if (!TryGetAttackDistances(entity, out float attackRange, out _))
            {
                return;
            }

            if (!IsInAttackRange(battleEntity, target, attackRange))
            {
                return;
            }

            _cooldownRemaining -= deltaTime;
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

        private static bool CanAttack(Entity entity)
        {
            if (!IsRunning(entity) || IsDead(entity))
            {
                return false;
            }

            if (!TryGetValidTarget(entity, out BattleEntity target))
            {
                return false;
            }

            var battleEntity = GetBattleEntity(entity);
            if (!TryGetAttackDistances(entity, out float attackRange, out _))
            {
                return false;
            }

            return IsInAttackRange(battleEntity, target, attackRange);
        }

        private static bool IsRunning(Entity entity)
        {
            return entity.GetAction<BattleWorldStateAction>().IsRunning;
        }

        private static bool IsDead(Entity entity)
        {
            var life = entity.GetData<BattleLifeData>();
            return life.IsDead;
        }

        private static BattleEntity GetBattleEntity(Entity entity)
        {
            return (BattleEntity)entity.SetupData[0];
        }

        private static bool TryGetValidTarget(Entity entity, out BattleEntity target)
        {
            var targetData = entity.GetData<TargetData>();
            target = targetData.Target;
            if (target == null)
            {
                return false;
            }

            var roster = entity.GetAction<BattleWorldRosterAction>();
            if (!roster.IsEntityAlive(target))
            {
                targetData.Target = null;
                target = null;
                return false;
            }

            return true;
        }

        private static bool TryGetAttackDistances(Entity entity, out float attackRange, out float stopDistance)
        {
            var distanceData = entity.GetData<BattleWorldAttackDistanceData>();
            attackRange = distanceData.AttackRange;
            stopDistance = distanceData.StopDistance;
            return true;
        }

        private static bool TryGetStatValue(Entity entity, string statId, out float value)
        {
            value = 0f;
            if (string.IsNullOrEmpty(statId))
            {
                return false;
            }

            var stats = entity.GetData<StatsEntityData>();
            return stats.TryGetStatValueById(statId, out value);
        }

        private static bool IsInAttackRange(BattleEntity attacker, BattleEntity target, float attackRange)
        {
            Vector3 current = attacker.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);
            return distance <= attackRange;
        }
    }
}
