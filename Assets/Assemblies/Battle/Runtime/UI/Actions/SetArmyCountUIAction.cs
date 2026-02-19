using System.Threading;
using ArmyClash.Battle.UI.Data;
using Cysharp.Threading.Tasks;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.UI.Actions
{
    [RunOnDirtyData(typeof(ArmyCountData))]
    [RequiresData(typeof(ArmyCountData), typeof(BattleUIViewData))]
    [Name("UI/ArmyClash/SetArmyCountUiAction")]
    public sealed class SetArmyCountUIAction : UIToolkitAction
    {
        protected override UniTask<bool> Run(CancellationToken token)
        {
            ArmyCountData data = Get<ArmyCountData>();
            BattleUIViewData view = Get<BattleUIViewData>();
            view.LeftCountLabel.text = data.LeftCount.ToString();
            view.RightCountLabel.text = data.RightCount.ToString();

            return UniTask.FromResult(true);
        }
    }
}
