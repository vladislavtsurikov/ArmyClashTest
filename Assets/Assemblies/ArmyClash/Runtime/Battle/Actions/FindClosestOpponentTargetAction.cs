using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(TargetData), typeof(BattleLifeData))]
    [Name("Battle/TargetAction")]
    public sealed class FindClosestOpponentTargetAction : CombatEntityAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            var stateAction = GetStateAction();
            var targetData = Get<TargetData>();
            var life = Get<BattleLifeData>();
            stateAction.IsRunningReactive
                .CombineLatest(targetData.TargetReactive, life.IsDeadReactive,
                    (running, target, isDead) => running && !isDead && target == null)
                .Where(canAcquire => canAcquire)
                .Subscribe(_ => TryAcquireTarget())
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }

        private void TryAcquireTarget()
        {
            var targetData = Get<TargetData>();
            var rosterAction = GetRosterAction();
            var target = FindClosestOpponent(Self, rosterAction);
            targetData.Target = target;
        }

        private static BattleEntity FindClosestOpponent(BattleEntity requester, BattleWorldRosterAction roster)
        {
            var team = requester.GetData<BattleTeamData>();
            System.Collections.Generic.IReadOnlyList<BattleEntity> list = team.TeamId == 0
                ? roster.RightEntities
                : roster.LeftEntities;

            if (list.Count == 0)
            {
                return null;
            }

            UnityEngine.Vector3 position = requester.transform.position;
            float bestSqr = float.MaxValue;
            BattleEntity best = null;

            for (int i = 0; i < list.Count; i++)
            {
                var candidate = list[i];
                if (candidate == null || !roster.IsEntityAlive(candidate))
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
