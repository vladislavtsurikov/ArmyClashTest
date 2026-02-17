using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldAutoRandomizeAction : EntityMonoBehaviourAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            var settings = Get<Data.BattleWorldAutoRandomizeData>();
            if (settings == null)
            {
                return;
            }

            settings.AutoRandomizeReactive
                .Where(enabled => enabled)
                .Take(1)
                .Subscribe(_ => Entity?.GetAction<BattleWorldSpawnAction>()?.RandomizeArmies())
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }
    }
}
