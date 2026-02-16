using UnityEngine.UIElements;

namespace ArmyClash.Battle.Ui
{
    public sealed class BattleUIView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BattleUIView, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        private BattleUIToolkitEntity _entity;
        private bool _initialized;

        public BattleUIToolkitEntity Entity
        {
            get
            {
                EnsureInitialized();
                return _entity;
            }
        }

        public BattleUIView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            EnsureInitialized();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            _entity?.Dispose();
            _entity = null;
            _initialized = false;
        }

        private void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            _entity = new BattleUIToolkitEntity(this);
        }
    }
}
