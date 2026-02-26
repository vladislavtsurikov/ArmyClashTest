using ArmyClash.Battle.Services;
using ArmyClash.Battle.UI.Data;
using ArmyClash.MegaWorldGrid;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.UI.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/RandomizeButtonAction")]
    public sealed class RandomizeButtonAction : UIToolkitAction
    {
        [Inject]
        private GridSpawnerPair _spawner;

        [Inject]
        private BattleStateService _state;

        protected override void OnFirstSetupComponentUI(object[] setupData = null)
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.RandomizeButton.clicked += OnClicked;
        }

        protected override void OnDisableElement()
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.RandomizeButton.clicked -= OnClicked;
        }

        private void OnClicked()
        {
            _state?.SetIdle();
            _spawner?.RespawnBoth();
        }
    }
}
