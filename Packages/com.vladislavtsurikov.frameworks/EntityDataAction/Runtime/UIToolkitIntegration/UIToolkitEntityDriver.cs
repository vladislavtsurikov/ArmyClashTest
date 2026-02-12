using System;
using UnityEngine.UIElements;

namespace VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration
{
    public sealed class UIToolkitEntityDriver
    {
        private readonly VisualElement _root;
        private readonly UIToolkitEntity _entity;
        private bool _isAttached;

        public UIToolkitEntity Entity => _entity;

        public UIToolkitEntityDriver(VisualElement root, Type[] dataTypes, Type[] actionTypes, int updateIntervalMs = 16,
            int fixedUpdateIntervalMs = 50)
        {
            _root = root;
            _entity = new UIToolkitEntity(dataTypes, actionTypes);
            _entity.SetSetupData(new object[] { _root });

            _root.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            _root.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            if (_root.panel != null)
            {
                _isAttached = true;
                EnableInternal();
            }
        }

        public void Dispose()
        {
            DisableInternal();

            _root.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            _root.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            if (_isAttached)
            {
                return;
            }

            _isAttached = true;

            EnableInternal();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            if (!_isAttached)
            {
                return;
            }

            _isAttached = false;

            DisableInternal();
        }

        private void EnableInternal()
        {
            _entity.Enable();
        }

        private void DisableInternal()
        {
            _entity.Disable();
        }
    }
}
