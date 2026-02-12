using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(RandomizeRequestData), typeof(BattleUiViewData))]
    [Name("UI/ArmyClash/RandomizeButtonAction")]
    public sealed class RandomizeButtonAction : UIToolkitAction
    {
        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            var view = Get<BattleUiViewData>();
            if (view != null && view.RandomizeButton != null)
            {
                view.RandomizeButton.clicked += OnClicked;
            }
        }

        protected override void OnDisable()
        {
            var view = Get<BattleUiViewData>();
            if (view != null && view.RandomizeButton != null)
            {
                view.RandomizeButton.clicked -= OnClicked;
            }
        }

        private void OnClicked()
        {
            RandomizeRequestData data = Get<RandomizeRequestData>();
            if (data == null)
            {
                return;
            }

            data.Request();
        }
    }
}
