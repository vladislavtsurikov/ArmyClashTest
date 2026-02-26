using ArmyClash.Battle.Services;
using ArmyClash.Battle.UI.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.UI.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/StartButtonAction")]
    public sealed class StartButtonAction : UIToolkitAction
    {
        [Inject]
        private BattleStateService _state;

        protected override void OnFirstSetupComponentUI(object[] setupData = null)
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.StartButton.clicked += OnClicked;
        }

        protected override void OnDisable()
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.StartButton.clicked -= OnClicked;
        }

        private void OnClicked() => _state?.StartBattle();
    }
}
