using ArmyClash.Battle.Data;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/AcquireTarget")]
    public sealed class BattleAcquireTargetState : State
    {
        protected override void Conditional()
        {
            var worldState = GetAction<BattleWorldStateAction>();
            var life = GetData<BattleLifeData>();
            var targetData = GetData<TargetData>();
            BindEligibility(worldState.IsRunningReactive
                .CombineLatest(life.IsDeadReactive, targetData.TargetReactive,
                    (running, isDead, target) => running && !isDead && target == null));
        }

        public override void Update(Entity entity, float deltaTime)
        {
            if (!IsRunning(entity) || IsDead(entity))
            {
                return;
            }

            var targetData = entity.GetData<TargetData>();
            var roster = entity.GetAction<BattleWorldRosterAction>();
            var battleEntity = GetBattleEntity(entity);

            if (targetData.Target == null)
            {
                targetData.Target = FindClosestOpponent(battleEntity, roster);
            }
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

        private static BattleEntity FindClosestOpponent(BattleEntity requester, BattleWorldRosterAction roster)
        {
            var team = requester.GetData<BattleTeamData>();
            System.Collections.Generic.IReadOnlyList<BattleEntity> list = team.TeamId == 0
                ? roster.RightEntities
                : roster.LeftEntities;

            Vector3 position = requester.transform.position;
            float bestSqr = float.MaxValue;
            BattleEntity best = null;

            for (int i = 0; i < list.Count; i++)
            {
                var candidate = list[i];
                if (!roster.IsEntityAlive(candidate))
                {
                    continue;
                }

                float sqr = (candidate.transform.position - position).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    best = candidate;
                }
            }

            return best;
        }
    }
}
