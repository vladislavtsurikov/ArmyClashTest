using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;
using Zenject;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/MoveToTarget")]
    public sealed class MoveToTargetState : State
    {
        [Inject]
        private BattleStateService _state;
        [Inject]
        private BattleTeamRoster _roster;

        protected override void Conditional()
        {
            BindEligibility(() => CanMove(Entity));
        }

        public override void Update(Entity entity, float deltaTime)
        {
            var life = entity.GetData<LifeData>();
            if (_state.SimulationState != SimulationState.Running || life.IsDead)
            {
                return;
            }

            var targetData = entity.GetData<TargetData>();
            var target = targetData.Target;
            if (target == null)
            {
                return;
            }

            var targetLife = target.GetData<LifeData>();
            if (targetLife.IsDead)
            {
                return;
            }

            var battleEntity = (BattleEntity)entity.SetupData[0];
            var ids = entity.GetData<StatIdsData>();
            if (string.IsNullOrEmpty(ids.SpeedId))
            {
                return;
            }

            var stats = entity.GetData<StatsEntityData>();
            if (!stats.TryGetStatValueById(ids.SpeedId, out float speed))
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

        private bool CanMove(Entity entity)
        {
            var life = entity.GetData<LifeData>();
            if (_state.SimulationState != SimulationState.Running || life.IsDead)
            {
                return false;
            }

            var targetData = entity.GetData<TargetData>();
            var target = targetData.Target;
            if (target == null)
            {
                return false;
            }

            var targetLife = target.GetData<LifeData>();
            if (targetLife.IsDead)
            {
                return false;
            }

            var battleEntity = (BattleEntity)entity.SetupData[0];
            if (!TryGetAttackDistances(entity, out float attackRange, out float stopDistance))
            {
                return false;
            }

            return IsOutsideStopRange(battleEntity, target, attackRange, stopDistance);
        }

        private static bool TryGetAttackDistances(Entity entity, out float attackRange, out float stopDistance)
        {
            attackRange = 0f;
            stopDistance = 0f;
            var distanceData = entity.GetData<AttackDistanceData>();
            attackRange = distanceData.AttackRange;
            stopDistance = distanceData.StopDistance;
            return true;
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
