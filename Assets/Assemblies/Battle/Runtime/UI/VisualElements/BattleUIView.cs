using UnityEngine;
using UnityEngine.UIElements;

namespace ArmyClash.Battle.Ui
{
    public sealed class BattleUIView : VisualElement
    {
        private BattleUIToolkitEntity _entity;
        private bool _initialized;

        public BattleUIView()
        {
            if (Application.isPlaying)
            {
                RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
                RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
            }
        }

        public BattleUIToolkitEntity Entity
        {
            get
            {
                EnsureInitialized();
                return _entity;
            }
        }

        private void OnAttachToPanel(AttachToPanelEvent evt) => EnsureInitialized();

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

        public new class UxmlFactory : UxmlFactory<BattleUIView, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }
    }
}
