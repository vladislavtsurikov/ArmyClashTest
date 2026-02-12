using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(ArmyCountData))]
    [RequiresData(typeof(ArmyCountData), typeof(BattleUiViewData))]
    [Name("UI/ArmyClash/SetArmyCountUiAction")]
    public sealed class SetArmyCountUiAction : UIToolkitAction
    {
        protected override UniTask<bool> Run(CancellationToken token)
        {
            var data = Get<ArmyCountData>();
            if (data == null)
            {
                return UniTask.FromResult(true);
            }

            var view = Get<BattleUiViewData>();
            if (view != null)
            {
                if (view.LeftCountLabel != null)
                {
                    view.LeftCountLabel.text = data.LeftCount.ToString();
                }

                if (view.RightCountLabel != null)
                {
                    view.RightCountLabel.text = data.RightCount.ToString();
                }
            }

            return UniTask.FromResult(true);
        }
    }
}
