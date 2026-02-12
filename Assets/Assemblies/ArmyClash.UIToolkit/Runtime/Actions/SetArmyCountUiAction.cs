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
    [RunOnDirtyData(typeof(ArmyCountData))]
    [RequiresData(typeof(ArmyCountData))]
    [Name("UI/ArmyClash/SetArmyCountUiAction")]
    public sealed class SetArmyCountUiAction : UIToolkitAction
    {
        [OdinSerialize]
        private string _leftCountName = "leftCount";

        [OdinSerialize]
        private string _rightCountName = "rightCount";

        private Label _leftLabel;
        private Label _rightLabel;

        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            _leftLabel = Query<Label>(_leftCountName);
            _rightLabel = Query<Label>(_rightCountName);
        }

        protected override UniTask<bool> Run(CancellationToken token)
        {
            var data = Get<ArmyCountData>();
            if (data == null)
            {
                return UniTask.FromResult(true);
            }

            if (_leftLabel != null)
            {
                _leftLabel.text = data.LeftCount.ToString();
            }

            if (_rightLabel != null)
            {
                _rightLabel.text = data.RightCount.ToString();
            }

            return UniTask.FromResult(true);
        }
    }
}
