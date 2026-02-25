using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;

namespace ArmyClash.Battle.Ui
{
    public sealed class BattleUIView : EntityUIView<BattleUIToolkitEntity>
    {
        protected override BattleUIToolkitEntity CreateEntity() => new BattleUIToolkitEntity(this);

        public new class UxmlFactory : UxmlFactory<BattleUIView, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }
    }
}
