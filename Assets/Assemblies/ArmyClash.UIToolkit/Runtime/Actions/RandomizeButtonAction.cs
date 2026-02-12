using OdinSerializer;
using UnityEngine.UIElements;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RequiresData(typeof(RandomizeRequestData))]
    [Name("UI/ArmyClash/RandomizeButtonAction")]
    public sealed class RandomizeButtonAction : UIToolkitAction
    {
        [OdinSerialize]
        private string _randomizeButtonName = "randomizeButton";

        private Button _button;

        protected override void OnFirstSetupComponentUi(object[] setupData = null)
        {
            _button = Query<Button>(_randomizeButtonName);
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
            RandomizeRequestData data = Get<RandomizeRequestData>();
            if (data == null)
            {
                return;
            }

            data.Request();
        }
    }
}
