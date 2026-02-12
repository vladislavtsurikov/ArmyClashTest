using System;
using UnityEngine.UIElements;

namespace VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration
{
    public sealed class UIToolkitEntityDriver
    {
        private readonly VisualElement _root;
        private readonly UIToolkitEntity _entity;
        private readonly int _updateIntervalMs;
        private readonly int _fixedUpdateIntervalMs;

        private IVisualElementScheduledItem _updateItem;
        private IVisualElementScheduledItem _fixedItem;
        private bool _isAttached;

        public UIToolkitEntity Entity => _entity;

        public UIToolkitEntityDriver(VisualElement root, Type[] dataTypes, Type[] actionTypes, int updateIntervalMs = 16,
            int fixedUpdateIntervalMs = 50)
        {
            _root = root;
            _entity = new UIToolkitEntity(dataTypes, actionTypes);
            _entity.SetSetupData(new object[] { _root });

            _updateIntervalMs = updateIntervalMs;
            _fixedUpdateIntervalMs = fixedUpdateIntervalMs;

            _root.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            _root.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            if (_root.panel != null)
            {
                _isAttached = true;
                _entity.Enable();
                StartSchedules();
            }
        }

        public void Dispose()
        {
            StopSchedules();

            _root.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            _root.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            _entity.Destroy();
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            if (_isAttached)
            {
                return;
            }

            _isAttached = true;

            _entity.Enable();
            StartSchedules();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            if (!_isAttached)
            {
                return;
            }

            _isAttached = false;

            StopSchedules();
            _entity.Disable();
        }

        private void StartSchedules()
        {
            _updateItem = _root.schedule.Execute(TickUpdate).Every(_updateIntervalMs);
            _fixedItem = _root.schedule.Execute(TickFixedUpdate).Every(_fixedUpdateIntervalMs);
        }

        private void StopSchedules()
        {
            _updateItem?.Pause();
            _fixedItem?.Pause();
        }

        private void TickUpdate()
        {
            _entity.Update();
            _entity.LateUpdate();
        }

        private void TickFixedUpdate()
        {
            _entity.FixedUpdate();
        }
    }
}
