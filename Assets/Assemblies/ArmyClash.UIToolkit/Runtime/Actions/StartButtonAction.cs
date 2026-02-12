using OdinSerializer;
using UnityEngine.UIElements;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(StartRequestData))]
    [Name("UI/ArmyClash/StartButtonAction")]
    public sealed class StartButtonAction : UIToolkitAction
    {
        [OdinSerialize]
        private string _startButtonName = "startButton";

        private Button _button;

        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            _button = Query<Button>(_startButtonName);
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

        private void OnClicked()
        {
            StartRequestData data = Get<StartRequestData>();
            if (data == null)
            {
                return;
            }

            data.Request();
        }
    }
}
