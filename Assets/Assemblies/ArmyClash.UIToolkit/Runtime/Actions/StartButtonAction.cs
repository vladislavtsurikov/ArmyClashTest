using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(StartRequestData), typeof(BattleUiViewData))]
    [Name("UI/ArmyClash/StartButtonAction")]
    public sealed class StartButtonAction : UIToolkitAction
    {
        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            var view = Get<BattleUiViewData>();
            if (view != null && view.StartButton != null)
            {
                view.StartButton.clicked += OnClicked;
            }
        }

        protected override void OnDisable()
        {
            var view = Get<BattleUiViewData>();
            if (view != null && view.StartButton != null)
            {
                view.StartButton.clicked -= OnClicked;
            }
        }

        private void OnClicked()
        {
            StartRequestData data = Get<StartRequestData>();
            if (data == null)
            {
                return;
            }

            data.Request();
        }
    }
}
