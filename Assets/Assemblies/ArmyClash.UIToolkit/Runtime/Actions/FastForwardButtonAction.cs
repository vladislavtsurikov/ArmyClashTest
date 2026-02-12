using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(BattleSpeedData))]
    [RequiresData(typeof(BattleSpeedData), typeof(BattleUiViewData))]
    [Name("UI/ArmyClash/FastForwardButtonAction")]
    public sealed class FastForwardButtonAction : UIToolkitAction
    {
        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            var view = Get<BattleUiViewData>();
            if (view != null && view.FastForwardButton != null)
            {
                view.FastForwardButton.clicked += OnClicked;
            }
        }

        protected override void OnDisable()
        {
            var view = Get<BattleUiViewData>();
            if (view != null && view.FastForwardButton != null)
            {
                view.FastForwardButton.clicked -= OnClicked;
            }
        }

        protected override Cysharp.Threading.Tasks.UniTask<bool> Run(System.Threading.CancellationToken token)
        {
            BattleSpeedData data = Get<BattleSpeedData>();
            if (data == null)
            {
                return Cysharp.Threading.Tasks.UniTask.FromResult(true);
            }

            UpdateVisual(data.Speed == BattleSpeed.Fast);
            return Cysharp.Threading.Tasks.UniTask.FromResult(true);
        }

        private void OnClicked()
        {
            BattleSpeedData data = Get<BattleSpeedData>();
            if (data == null)
            {
                return;
            }

            data.Speed = data.Speed == BattleSpeed.Fast ? BattleSpeed.Normal : BattleSpeed.Fast;
        }

        private void UpdateVisual(bool isFast)
        {
            var view = Get<BattleUiViewData>();
            var button = view != null ? view.FastForwardButton : null;
            if (button == null)
            {
                return;
            }

            if (isFast)
            {
                button.AddToClassList("is-fast");
            }
            else
            {
                button.RemoveFromClassList("is-fast");
            }
        }
    }
}
