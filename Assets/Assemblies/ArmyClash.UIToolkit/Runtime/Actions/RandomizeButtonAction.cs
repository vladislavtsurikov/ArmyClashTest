using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(RandomizeRequestData), typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/RandomizeButtonAction")]
    public sealed class RandomizeButtonAction : UIToolkitAction
    {
        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            var view = Get<BattleUIViewData>();
            view.RandomizeButton.clicked += OnClicked;
        }

        protected override void OnDisable()
        {
            var view = Get<BattleUIViewData>();
            view.RandomizeButton.clicked -= OnClicked;
        }

        private void OnClicked()
        {
            RandomizeRequestData data = Get<RandomizeRequestData>();
            data.Request();
        }
    }
}
