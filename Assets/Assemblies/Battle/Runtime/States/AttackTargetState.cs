using System;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UniRx;
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
        private const string AttackId = "ATK";
        private const string AttackSpeedId = "ATKSPD";
        private const string HealthId = "HP";

        private float _cooldownRemaining;

        [Inject]
        private BattleTeamRoster _roster;

        [Inject]
        private BattleStateService _state;

        protected override void Conditional()
        {
            IObservable<bool> canAttack =
                Observable.EveryUpdate()
                    .Select(_ => CanAttack())
                    .DistinctUntilChanged();

            BindEligibility(canAttack);

            canAttack
                .Select(active =>
                    active
                        ? Observable.EveryUpdate()
                        : Observable.Empty<long>())
                .Switch()
                .Subscribe(_ => AttackStep())
                .AddTo(Subscriptions);
        }

        private bool CanAttack()
        {
            LifeData life = Entity.GetData<LifeData>();
            if (_state.SimulationState != SimulationState.Running || life.IsDead.Value)
            {
                return false;
            }

            TargetData targetData = Entity.GetData<TargetData>();
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

            BattleEntity battleEntity = (BattleEntity)Entity;
            AttackDistanceData distanceData = Entity.GetData<AttackDistanceData>();
            float attackRange = distanceData.AttackRange.Value;

            return IsInAttackRange(battleEntity, target, attackRange);
        }

        private void AttackStep()
        {
            EntityMonoBehaviour entity = Entity;
            if (entity == null)
            {
                return;
            }

            TargetData targetData = entity.GetData<TargetData>();
            BattleEntity target = targetData.Target.Value;
            if (target == null)
            {
                return;
            }

            BattleEntity battleEntity = (BattleEntity)Entity;
            StatsEntityData stats = entity.GetData<StatsEntityData>();

            float attack = stats.GetStatValueById(AttackId);
            float attackSpeed = stats.GetStatValueById(AttackSpeedId);

            AttackDistanceData distanceData = entity.GetData<AttackDistanceData>();
            float attackRange = distanceData.AttackRange.Value;

            if (!IsInAttackRange(battleEntity, target, attackRange))
            {
                return;
            }

            _cooldownRemaining -= Time.deltaTime;
            if (_cooldownRemaining > 0f)
            {
                return;
            }

            _cooldownRemaining = Mathf.Max(0.01f, attackSpeed);

            StatsEntityData targetStats = target.GetData<StatsEntityData>();
            targetStats.AddStatValueById(HealthId, -attack);
        }

        private bool IsInAttackRange(BattleEntity attacker, BattleEntity target, float attackRange)
        {
            Vector3 current = attacker.transform.position;
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(current, targetPosition);
            return distance <= attackRange;
        }
    }
}
