using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(BattleLifeData))]
    [Name("Action/HandleDeath")]
    public sealed class HandleDeathAction : CombatEntityAction
    {
        private BattleLifeData _life;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected override void OnEnable()
        {
            _life = Get<BattleLifeData>();
            _subscriptions.Clear();
            _life.IsDeadReactive
                .Subscribe(HandleIsDeadChanged)
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
            _life = null;
        }

        private void HandleIsDeadChanged(bool isDead)
        {
            if (!isDead)
            {
                return;
            }

            var stateAction = GetStateAction();
            if (!stateAction.IsRunning)
            {
                return;
            }

            stateAction.HandleEntityDeath(Self);
        }
    }
}
