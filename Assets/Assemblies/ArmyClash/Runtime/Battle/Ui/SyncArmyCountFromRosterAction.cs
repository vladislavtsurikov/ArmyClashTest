using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.Battle.Services;
using ArmyClash.UIToolkit.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(ArmyCountData))]
    [Name("UI/ArmyClash/SyncArmyCountFromRosterAction")]
    public sealed class SyncArmyCountFromRosterAction : UIToolkitAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        [Inject]
        private BattleTeamRoster _roster;

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            _roster.RosterChanged
                .Subscribe(_ => UpdateArmyCount())
                .AddTo(_subscriptions);

            UpdateArmyCount();
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }

        protected override UniTask<bool> Run(CancellationToken token)
        {
            UpdateArmyCount();
            return UniTask.FromResult(true);
        }

        private void UpdateArmyCount()
        {
            var data = Get<ArmyCountData>();
            data.LeftCount = _roster.LeftCount;
            data.RightCount = _roster.RightCount;
        }
    }
}
