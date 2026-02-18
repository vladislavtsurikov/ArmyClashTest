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
    [Name("Battle/StateMachine/AttackTarget")]
    public sealed class AttackTargetState : State
    {
        private float _cooldownRemaining;

        [Inject]
        private BattleStateService _state;
        [Inject]
        private BattleTeamRoster _roster;

        protected override void Conditional()
        {
            BindEligibility(() => CanAttack(Entity));
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
            if (string.IsNullOrEmpty(ids.AttackId))
            {
                return;
            }

            if (string.IsNullOrEmpty(ids.AttackSpeedId))
            {
                return;
            }

            var stats = entity.GetData<StatsEntityData>();
            if (!stats.TryGetStatValueById(ids.AttackId, out float attack))
            {
                return;
            }

            if (!stats.TryGetStatValueById(ids.AttackSpeedId, out float attackSpeed))
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

        private bool CanAttack(Entity entity)
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
            if (!TryGetAttackDistances(entity, out float attackRange, out _))
            {
                return false;
            }

            return IsInAttackRange(battleEntity, target, attackRange);
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


        private static bool IsInAttackRange(BattleEntity attacker, BattleEntity target, float attackRange)
        {
            Vector3 current = attacker.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);
            return distance <= attackRange;
        }
    }
}
