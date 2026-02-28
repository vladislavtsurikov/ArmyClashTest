using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using ArmyClash.Battle.UI.Data;
using UniRx;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.UI.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/SetButtonsVisibilityAction")]
    public sealed class SetButtonsVisibilityAction : UIToolkitAction
    {
        private readonly CompositeDisposable _subscriptions = new();

        [Inject]
        private BattleStateService _state;

        protected override void SetupComponent(object[] setupData = null)
        {
            _subscriptions.Clear();

            if (_state == null)
            {
                return;
            }

            _state.SimulationStateReactive
                .Subscribe(ApplyState)
                .AddTo(_subscriptions);

            ApplyState(_state.SimulationState);
        }

        protected override void OnDisableElement() => _subscriptions.Clear();

        private void ApplyState(SimulationState state)
        {
            bool showSetupButtons = state != SimulationState.Running;
            bool showFastForwardButton = state == SimulationState.Running;
            BattleUIViewData view = Get<BattleUIViewData>();
            SetVisible(view.RandomizeButton, showSetupButtons);
            SetVisible(view.StartButton, showSetupButtons);
            SetVisible(view.FastForwardButton, showFastForwardButton);
        }

        private static void SetVisible(VisualElement element, bool visible) =>
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
