using System.Threading;
using ArmyClash.Battle.UI.Data;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.UI.Actions
{
    [RunOnDirtyData(typeof(BattleSpeedData))]
    [RequiresData(typeof(BattleSpeedData), typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/FastForwardButtonAction")]
    public sealed class FastForwardButtonAction : UIToolkitAction
    {
        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.FastForwardButton.clicked += OnClicked;
        }

        protected override void OnDisable()
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            view.FastForwardButton.clicked -= OnClicked;
        }

        protected override UniTask<bool> Run(CancellationToken token)
        {
            BattleSpeedData data = Get<BattleSpeedData>();
            UpdateVisual(data.Speed == BattleSpeed.Fast);
            return UniTask.FromResult(true);
        }

        private void OnClicked()
        {
            BattleSpeedData data = Get<BattleSpeedData>();
            data.Speed = data.Speed == BattleSpeed.Fast ? BattleSpeed.Normal : BattleSpeed.Fast;
        }

        private void UpdateVisual(bool isFast)
        {
            BattleUIViewData view = Get<BattleUIViewData>();
            Button button = view.FastForwardButton;

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
