using ArmyClash.Battle.Data;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/MoveToTarget")]
    public sealed class BattleMoveToTargetState : State
    {
        protected override void Conditional()
        {
            BindEligibility(() => CanMove(Entity));
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
            if (!TryGetStatValue(entity, ids.SpeedId, out float speed))
            {
                return;
            }

            if (!TryGetAttackDistances(entity, out float attackRange, out float stopDistance))
            {
                return;
            }

            Vector3 current = battleEntity.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 direction = targetPosition - current;
            float distance = direction.magnitude;

            if (distance <= Mathf.Max(0f, attackRange - stopDistance))
            {
                return;
            }

            Vector3 next = Vector3.MoveTowards(current, targetPosition, speed * deltaTime);
            battleEntity.transform.position = next;
        }

        private static bool CanMove(Entity entity)
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
            if (!TryGetAttackDistances(entity, out float attackRange, out float stopDistance))
            {
                return false;
            }

            return IsOutsideStopRange(battleEntity, target, attackRange, stopDistance);
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
            attackRange = 0f;
            stopDistance = 0f;
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

        private static bool IsOutsideStopRange(BattleEntity mover, BattleEntity target, float attackRange, float stopDistance)
        {
            Vector3 current = mover.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);
            return distance > Mathf.Max(0f, attackRange - stopDistance);
        }
    }
}
