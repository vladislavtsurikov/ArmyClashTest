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
        private const string SpeedId = "SPEED";

        [Inject]
        private BattleTeamRoster _roster;

        [Inject]
        private BattleStateService _state;

        protected override bool Conditional() => CanMove();

        protected override void Tick(float deltaTime) => MoveStep();

        private bool CanMove()
        {
            LifeData life = EntityMonoBehaviour.GetData<LifeData>();
            if (_state.SimulationState != SimulationState.Running || life.IsDead.Value)
            {
                return false;
            }

            TargetData targetData = EntityMonoBehaviour.GetData<TargetData>();
            BattleEntity target = targetData.Target.Value;
            if (target == null)
            {
                return false;
            }

            LifeData targetLife = target.GetData<LifeData>();
            if (targetLife.IsDead.Value)
            {
                return false;
            }

            BattleEntity battleEntity = (BattleEntity)EntityMonoBehaviour;
            AttackDistanceData distanceData = EntityMonoBehaviour.GetData<AttackDistanceData>();
            float attackRange = distanceData.AttackRange.Value;
            float stopDistance = distanceData.StopDistance.Value;

            return IsOutsideStopRange(battleEntity, target, attackRange, stopDistance);
        }

        private void MoveStep()
        {
            EntityMonoBehaviour entity = EntityMonoBehaviour;
            if (entity == null)
            {
                return;
            }

            StatsEntityData stats = entity.GetData<StatsEntityData>();
            float speed = stats.GetStatValueById(SpeedId);

            AttackDistanceData distanceData = entity.GetData<AttackDistanceData>();
            float attackRange = distanceData.AttackRange.Value;
            float stopDistance = distanceData.StopDistance.Value;

            TargetData targetData = entity.GetData<TargetData>();
            BattleEntity target = targetData.Target.Value;
            if (target == null)
            {
                return;
            }

            BattleEntity battleEntity = (BattleEntity)EntityMonoBehaviour;
            Vector3 current = battleEntity.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 direction = targetPosition - current;
            float distance = direction.magnitude;

            if (distance > Mathf.Max(0f, attackRange - stopDistance))
            {
                Vector3 next = Vector3.MoveTowards(current, targetPosition, speed * Time.deltaTime);
                battleEntity.transform.position = next;
            }
        }

        private bool IsOutsideStopRange(BattleEntity mover, BattleEntity target, float attackRange,
            float stopDistance)
        {
            Vector3 current = mover.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);
            return distance > Mathf.Max(0f, attackRange - stopDistance);
        }
    }
}
