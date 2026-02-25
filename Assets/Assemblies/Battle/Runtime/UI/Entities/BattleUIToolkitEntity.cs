using System;
using ArmyClash.Battle.UI.Actions;
using ArmyClash.Battle.UI.Data;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;

namespace ArmyClash.Battle.Ui
{
    public sealed class BattleUIToolkitEntity : UIToolkitEntity, IDisposable
    {
        public BattleUIToolkitEntity(VisualElement root) : base(root)
        {
        }

        protected override Type[] ComponentDataTypesToCreate() =>
            new[] { typeof(ArmyCountData), typeof(BattleSpeedData), typeof(BattleUIViewData) };

        protected override Type[] ActionTypesToCreate() =>
            new[]
            {
                typeof(StartButtonAction), typeof(RandomizeButtonAction), typeof(SetButtonsVisibilityAction),
                typeof(SetArmyCountUIAction), typeof(FastForwardButtonAction), typeof(ApplySpeedTimeScaleAction),
                typeof(SyncArmyCountFromRosterAction)
            };
    }
}
