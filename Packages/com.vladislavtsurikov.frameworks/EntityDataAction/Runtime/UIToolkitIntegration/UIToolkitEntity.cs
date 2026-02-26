using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration
{
    public class UIToolkitEntity : Entity
    {
        private bool _isAttached;

        public UIToolkitEntity(VisualElement root)
        {
            Root = root;

            SetSetupData(new object[] { Root });

            Root.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            Root.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            if (Root.panel != null)
            {
                _isAttached = true;
                Setup();
            }
        }

        public VisualElement Root { get; }

        public void Dispose()
        {
            Root.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            Root.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            if (_isAttached)
            {
                return;
            }

            _isAttached = true;

            Setup();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            if (!_isAttached)
            {
                return;
            }

            _isAttached = false;

            Disable();
        }
    }
}
