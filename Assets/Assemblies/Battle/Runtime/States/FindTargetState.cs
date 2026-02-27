using System.Collections.Generic;
using System.Linq;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
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

        protected override bool Conditional()
        {
            LifeData life = GetData<LifeData>();
            TargetData targetData = GetData<TargetData>();

            bool running = _state.SimulationState == SimulationState.Running;
            bool isDead = life.IsDead.Value;
            bool hasTarget = targetData.Target.Value != null;

            return running && !isDead && !hasTarget;
        }

        protected override void Tick(float deltaTime) => FindClosestOpponent();

        private void FindClosestOpponent()
        {
            TargetData targetData = EntityMonoBehaviour.GetData<TargetData>();

            TeamData team = EntityMonoBehaviour.GetData<TeamData>();

            IReadOnlyList<EntityMonoBehaviour> list = team.TeamId == 0
                ? _roster.RightEntities
                : _roster.LeftEntities;

            Vector3 position = EntityMonoBehaviour.transform.position;

            targetData.Target.Value = list
                .Where(candidate => !candidate.GetData<LifeData>().IsDead.Value)
                .Cast<BattleEntity>()
                .OrderBy(candidate => (candidate.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}
