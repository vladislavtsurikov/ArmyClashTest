using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using ArmyClash.UIToolkit.Data;
using UniRx;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using Zenject;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/SetButtonsVisibilityAction")]
    public sealed class SetButtonsVisibilityAction : UIToolkitAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        [Inject]
        private BattleStateService _state;

        protected override void OnEnable()
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

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }

        private void ApplyState(SimulationState state)
        {
            bool show = state != SimulationState.Running;
            var view = Get<BattleUIViewData>();
            SetVisible(view.RandomizeButton, show);
            SetVisible(view.StartButton, show);
        }

        private static void SetVisible(VisualElement element, bool visible)
        {
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
