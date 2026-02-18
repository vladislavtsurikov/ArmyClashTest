using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using ArmyClash.Battle.Services;
using ArmyClash.MegaWorldGrid;
using Zenject;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/RandomizeButtonAction")]
    public sealed class RandomizeButtonAction : UIToolkitAction
    {
        [Inject]
        private BattleStateService _state;
        [Inject]
        private GridSpawnerPair _spawnerPair;

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
            _state?.SetIdle();
            _spawnerPair?.RandomizeArmies();
        }
    }
}
