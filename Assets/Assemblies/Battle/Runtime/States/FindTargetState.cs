using System.Collections.Generic;
using System.Linq;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;
using Zenject;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/FindTarget")]
    public sealed class FindTargetState : State
    {
        [Inject]
        private BattleTeamRoster _roster;

        [Inject]
        private BattleStateService _state;

        protected override void Conditional()
        {
            LifeData life = GetData<LifeData>();
            TargetData targetData = GetData<TargetData>();

            var canAcquire = _state.SimulationStateReactive
                .Select(state => state == SimulationState.Running)
                .CombineLatest(life.IsDeadReactive, targetData.TargetReactive,
                    (running, isDead, target) => running && !isDead && target == null);

            BindEligibility(canAcquire);

            canAcquire
                .Where(can => can)
                .Subscribe(_ => FindClosestOpponent())
                .AddTo(Subscriptions);
        }

        private void FindClosestOpponent()
        {
            TargetData targetData = Entity.GetData<TargetData>();

            TeamData team = Entity.GetData<TeamData>();

            IReadOnlyList<EntityMonoBehaviour> list = team.TeamId == 0
                ? _roster.RightEntities
                : _roster.LeftEntities;

            Vector3 position = Entity.transform.position;

            targetData.Target = list
                .Where(candidate => !candidate.GetData<LifeData>().IsDead)
                .Cast<BattleEntity>()
                .OrderBy(candidate => (candidate.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}
