using System.Threading;
using ArmyClash.Battle.Services;
using ArmyClash.Battle.UI.Data;
using Cysharp.Threading.Tasks;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.UI.Actions
{
    [RequiresData(typeof(ArmyCountData))]
    [Name("UI/ArmyClash/SyncArmyCountFromRosterAction")]
    public sealed class SyncArmyCountFromRosterAction : UIToolkitAction
    {
        private readonly CompositeDisposable _subscriptions = new();

        [Inject]
        private BattleTeamRoster _roster;

        protected override void SetupComponent(object[] setupData = null)
        {
            _subscriptions.Clear();

            _roster.RosterChanged
                .Subscribe(_ => UpdateArmyCount())
                .AddTo(_subscriptions);

            UpdateArmyCount();
        }

        protected override void OnDisableElement() => _subscriptions.Clear();

        protected override UniTask<bool> Run(CancellationToken token)
        {
            UpdateArmyCount();
            return UniTask.FromResult(true);
        }

        private void UpdateArmyCount()
        {
            ArmyCountData data = Get<ArmyCountData>();
            data.LeftCount = _roster.LeftCount;
            data.RightCount = _roster.RightCount;
        }
    }
}
