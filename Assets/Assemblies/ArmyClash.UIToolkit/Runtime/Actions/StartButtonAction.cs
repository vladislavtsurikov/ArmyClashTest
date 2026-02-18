using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using ArmyClash.Battle.Services;
using Zenject;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/StartButtonAction")]
    public sealed class StartButtonAction : UIToolkitAction
    {
        [Inject]
        private BattleStateService _state;

        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            var view = Get<BattleUIViewData>();
            view.StartButton.clicked += OnClicked;
        }

        protected override void OnDisable()
        {
            var view = Get<BattleUIViewData>();
            view.StartButton.clicked -= OnClicked;
        }

        private void OnClicked()
        {
            _state?.StartBattle();
        }
    }
}
