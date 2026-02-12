namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public partial class Entity
    {
        protected void OnEnable()
        {
            Setup();

            if (!Active)
            {
                return;
            }

            if (_actions != null)
            {
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
        }

        protected void OnDisable()
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

        protected void Update()
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeUpdate();
        }

        protected void FixedUpdate()
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeFixedUpdate();
        }

        protected void LateUpdate()
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeLateUpdate();
        }

        protected void OnDestroy()
        {
            _actions?.InvokeOnDestroy();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            _actions?.InvokeOnValidate();
        }
#endif

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeOnApplicationFocus(hasFocus);
        }

        protected void OnApplicationPause(bool pauseStatus)
        {
            if (!Active)
            {
                return;
            }

            _actions?.InvokeOnApplicationPause(pauseStatus);
        }
    }
}
