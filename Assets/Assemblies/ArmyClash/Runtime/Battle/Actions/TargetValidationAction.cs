using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(TargetData))]
    [Name("Action/TargetValidation")]
    public sealed class TargetValidationAction : CombatEntityAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private readonly SerialDisposable _targetSubscription = new SerialDisposable();

        protected override void OnEnable()
        {
            _subscriptions.Clear();
            _targetSubscription.Disposable = null;

            var targetData = Get<TargetData>();
            targetData.TargetReactive
                .Subscribe(TrackTarget)
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
            _targetSubscription.Disposable = null;
        }

        private void TrackTarget(BattleEntity target)
        {
            _targetSubscription.Disposable = null;

            var targetData = Get<TargetData>();
            if (target == null)
            {
                return;
            }

            var life = target.GetData<BattleLifeData>();
            _targetSubscription.Disposable = life.IsDeadReactive
                .Where(isDead => isDead)
                .Subscribe(_ =>
                {
                    if (targetData.Target == target)
                    {
                        targetData.Target = null;
                    }
                });
        }
    }
}
