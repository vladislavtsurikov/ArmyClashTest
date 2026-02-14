using System;
using ArmyClash.UIToolkit.Actions;
using ArmyClash.UIToolkit.Data;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;

namespace ArmyClash.Battle.Ui
{
    public sealed class BattleUIToolkitEntity : UIToolkitEntity
    {
        public BattleUIToolkitEntity(VisualElement root) : base(root)
        {
        }

        protected override Type[] ComponentDataTypesToCreate()
        {
            return new[]
            {
                typeof(StartRequestData),
                typeof(RandomizeRequestData),
                typeof(SimulationStateData),
                typeof(ArmyCountData),
                typeof(BattleSpeedData),
                typeof(BattleUIViewData)
            };
        }

        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(StartButtonAction),
                typeof(RandomizeButtonAction),
                typeof(SetButtonsVisibilityAction),
                typeof(SetArmyCountUIAction),
                typeof(FastForwardButtonAction)
            };
        }
    }
}
