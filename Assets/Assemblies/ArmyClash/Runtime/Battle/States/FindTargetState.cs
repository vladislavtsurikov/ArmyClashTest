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
        private BattleStateService _state;
        [Inject]
        private BattleTeamRoster _roster;

        protected override void Conditional()
        {
            var life = GetData<LifeData>();
            var targetData = GetData<TargetData>();

            BindEligibility(_state.SimulationStateReactive
                .Select(state => state == SimulationState.Running)
                .CombineLatest(life.IsDeadReactive, targetData.TargetReactive,
                    (running, isDead, target) => running && !isDead && target == null));
        }

        public override void Update(Entity entity, float deltaTime)
        {
            var life = entity.GetData<LifeData>();
            if (_state.SimulationState != SimulationState.Running || life.IsDead)
            {
                return;
            }

            var targetData = entity.GetData<TargetData>();
            var battleEntity = (BattleEntity)entity.SetupData[0];

            if (targetData.Target == null)
            {
                targetData.Target = FindClosestOpponent(battleEntity, _roster);
            }
        }

        private static BattleEntity FindClosestOpponent(BattleEntity requester, BattleTeamRoster roster)
        {
            var team = requester.GetData<TeamData>();

            System.Collections.Generic.IReadOnlyList<EntityMonoBehaviour> list = team.TeamId == 0
                ? roster.RightEntities
                : roster.LeftEntities;

            Vector3 position = requester.transform.position;

            return list
                .Where(candidate => !candidate.GetData<LifeData>().IsDead)
                .Cast<BattleEntity>()
                .OrderBy(candidate => (candidate.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}
