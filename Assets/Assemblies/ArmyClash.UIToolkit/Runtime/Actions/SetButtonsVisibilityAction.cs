using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(SimulationStateData))]
    [RequiresData(typeof(SimulationStateData), typeof(BattleUiViewData))]
    [Name("UI/ArmyClash/SetButtonsVisibilityAction")]
    public sealed class SetButtonsVisibilityAction : UIToolkitAction
    {
        protected override UniTask<bool> Run(CancellationToken token)
        {
            SimulationStateData data = Get<SimulationStateData>();
            if (data == null)
            {
                return UniTask.FromResult(true);
            }

            bool show = data.State != SimulationState.Running;

            var view = Get<BattleUiViewData>();
            if (view != null)
            {
                SetVisible(view.RandomizeButton, show);
                SetVisible(view.StartButton, show);
            }

            return UniTask.FromResult(true);
        }

        private static void SetVisible(VisualElement element, bool visible)
        {
            if (element == null)
            {
                return;
            }

            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
