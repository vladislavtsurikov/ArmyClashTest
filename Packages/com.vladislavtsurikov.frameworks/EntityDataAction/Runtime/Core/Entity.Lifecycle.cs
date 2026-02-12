namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public partial class Entity
    {
        public void Enable()
        {
            Setup();

            if (!Active)
            {
                return;
            }

            if (!_actionsAwakeCalled)
            {
                _actionsAwakeCalled = true;
                _actions.InvokeAwake();
            }

            _actions.InvokeOnEnable();

            if (!_actionsStartCalled)
            {
                _actionsStartCalled = true;
                _actions.InvokeStart();
            }
        }

        public void Disable()
        {
            _actions?.InvokeOnDisable();

            EntityDataActionGlobalSettings.ActiveChanged -= HandleActiveChanged;
            _data.ElementAdded -= HandleDataChanged;
            _data.ElementRemoved -= HandleDataChanged;

            _data.OnDisable();
            _actions.OnDisable();

            DirtyRunner?.OnDisable();

            IsSetup = false;
        }

        public void Update()
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeUpdate();
        }

        public void FixedUpdate()
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeFixedUpdate();
        }

        public void LateUpdate()
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeLateUpdate();
        }

        public void Destroy()
        {
            _actions?.InvokeOnDestroy();
        }

#if UNITY_EDITOR
        public void Validate()
        {
            _actions?.InvokeOnValidate();
        }
#endif

        public void OnApplicationFocus(bool hasFocus)
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeOnApplicationFocus(hasFocus);
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeOnApplicationPause(pauseStatus);
        }
    }
}
