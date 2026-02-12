using System.Threading;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using UnityEngine.UIElements;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(SimulationStateData))]
    [RequiresData(typeof(SimulationStateData))]
    [Name("UI/ArmyClash/SetButtonsVisibilityAction")]
    public sealed class SetButtonsVisibilityAction : UIToolkitAction
    {
        [OdinSerialize]
        private string _randomizeButtonName = "randomizeButton";

        [OdinSerialize]
        private string _startButtonName = "startButton";

        private VisualElement _randomizeButton;
        private VisualElement _startButton;

        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            _randomizeButton = Query<VisualElement>(_randomizeButtonName);
            _startButton = Query<VisualElement>(_startButtonName);
        }

        protected override UniTask<bool> Run(CancellationToken token)
        {
            SimulationStateData data = Get<SimulationStateData>();
            if (data == null)
            {
                return UniTask.FromResult(true);
            }

            bool show = data.State != SimulationState.Running;

            SetVisible(_randomizeButton, show);
            SetVisible(_startButton, show);

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
