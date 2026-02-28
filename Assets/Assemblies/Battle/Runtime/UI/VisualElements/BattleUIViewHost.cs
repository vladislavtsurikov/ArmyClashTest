using ArmyClash.Battle.Ui;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;

namespace ArmyClash.Battle.UI.VisualElements
{
    public sealed class BattleUIViewHost : UIToolkitEntityHost<BattleUIToolkitEntity>
    {
        protected override BattleUIToolkitEntity CreateEntity(VisualElement root) => new BattleUIToolkitEntity(root);
    }
}
