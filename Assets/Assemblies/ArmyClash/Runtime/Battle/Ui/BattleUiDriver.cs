using System;
using UnityEngine;
using UnityEngine.UIElements;
using ArmyClash.UIToolkit.Actions;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;

namespace ArmyClash.Battle.Ui
{
    public sealed class BattleUiDriver : MonoBehaviour
    {
        [SerializeField]
        private UIDocument _document;

        [SerializeField]
        private int _updateIntervalMs = 16;

        [SerializeField]
        private int _fixedUpdateIntervalMs = 50;

        private UIToolkitEntityDriver _driver;

        public UIToolkitEntity Entity => _driver != null ? _driver.Entity : null;

        private void OnEnable()
        {
            if (_document == null)
            {
                _document = GetComponent<UIDocument>();
            }

            if (_document == null)
            {
                return;
            }

            var root = _document.rootVisualElement;
            if (root == null)
            {
                return;
            }

            _driver = new UIToolkitEntityDriver(root, GetDataTypes(), GetActionTypes(), _updateIntervalMs, _fixedUpdateIntervalMs);
        }

        private void OnDisable()
        {
            _driver?.Dispose();
            _driver = null;
        }

        private static Type[] GetDataTypes()
        {
            return new[]
            {
                typeof(StartRequestData),
                typeof(RandomizeRequestData),
                typeof(SimulationStateData),
                typeof(ArmyCountData),
                typeof(BattleSpeedData),
                typeof(BattleUiViewData)
            };
        }

        private static Type[] GetActionTypes()
        {
            return new[]
            {
                typeof(StartButtonAction),
                typeof(RandomizeButtonAction),
                typeof(SetButtonsVisibilityAction),
                typeof(SetArmyCountUiAction),
                typeof(FastForwardButtonAction)
            };
        }
    }
}
