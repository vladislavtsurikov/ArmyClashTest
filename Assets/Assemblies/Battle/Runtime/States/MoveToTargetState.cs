using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UniRx;
using UnityEngine;
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

        protected override void Conditional()
        {
            var canMove =
                Observable.EveryUpdate()
                    .Select(_ => CanMove())
                    .DistinctUntilChanged();

            BindEligibility(canMove);

            canMove
                .Select(active =>
                    active
                        ? Observable.EveryUpdate()
                        : Observable.Empty<long>())
                .Switch()
                .Subscribe(_ => MoveStep())
                .AddTo(Subscriptions);
        }

        private bool CanMove()
        {
            LifeData life = Entity.GetData<LifeData>();
            if (_state.SimulationState != SimulationState.Running || life.IsDead)
            {
                return false;
            }

            TargetData targetData = Entity.GetData<TargetData>();
            BattleEntity target = targetData.Target;
            if (target == null)
            {
                return false;
            }

            LifeData targetLife = target.GetData<LifeData>();
            if (targetLife.IsDead)
            {
                return false;
            }

            var battleEntity = (BattleEntity)Entity;
            var distanceData = Entity.GetData<AttackDistanceData>();
            float attackRange = distanceData.AttackRange;
            float stopDistance = distanceData.StopDistance;

            return IsOutsideStopRange(battleEntity, target, attackRange, stopDistance);
        }

        private void MoveStep()
        {
            var entity = Entity;
            if (entity == null)
            {
                return;
            }

            StatsEntityData stats = entity.GetData<StatsEntityData>();
            float speed = stats.GetStatValueById(SpeedId);

            var distanceData = entity.GetData<AttackDistanceData>();
            float attackRange = distanceData.AttackRange;
            float stopDistance = distanceData.StopDistance;

            TargetData targetData = entity.GetData<TargetData>();
            BattleEntity target = targetData.Target;
            if (target == null)
            {
                return;
            }

            var battleEntity = (BattleEntity)Entity;
            Vector3 current = battleEntity.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 direction = targetPosition - current;
            var distance = direction.magnitude;

            if (distance > Mathf.Max(0f, attackRange - stopDistance))
            {
                var next = Vector3.MoveTowards(current, targetPosition, speed * Time.deltaTime);
                battleEntity.transform.position = next;
            }
        }

        private bool IsOutsideStopRange(BattleEntity mover, BattleEntity target, float attackRange,
            float stopDistance)
        {
            Vector3 current = mover.transform.position;
            Vector3 targetPosition = target.transform.position;
            var distance = Vector3.Distance(current, targetPosition);
            return distance > Mathf.Max(0f, attackRange - stopDistance);
        }
    }
}
