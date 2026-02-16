using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(ArmyCountData))]
    [RequiresData(typeof(ArmyCountData), typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/SetArmyCountUiAction")]
    public sealed class SetArmyCountUIAction : UIToolkitAction
    {
        protected override UniTask<bool> Run(CancellationToken token)
        {
            var data = Get<ArmyCountData>();
            var view = Get<BattleUIViewData>();
            view.LeftCountLabel.text = data.LeftCount.ToString();
            view.RightCountLabel.text = data.RightCount.ToString();

            return UniTask.FromResult(true);
        }
    }
}
