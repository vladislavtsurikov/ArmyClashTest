using OdinSerializer;
using UnityEngine.UIElements;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(BattleSpeedData))]
    [RequiresData(typeof(BattleSpeedData))]
    [Name("UI/ArmyClash/FastForwardButtonAction")]
    public sealed class FastForwardButtonAction : UIToolkitAction
    {
        [OdinSerialize]
        private string _fastForwardButtonName = "fastForwardButton";

        private Button _button;

        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            _button = Query<Button>(_fastForwardButtonName);
            if (_button != null)
            {
                _button.clicked += OnClicked;
            }
        }

        protected override void OnDisable()
        {
            if (_button != null)
            {
                _button.clicked -= OnClicked;
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
            if (_button == null)
            {
                return;
            }

            if (isFast)
            {
                _button.AddToClassList("is-fast");
            }
            else
            {
                _button.RemoveFromClassList("is-fast");
            }
        }
    }
}
