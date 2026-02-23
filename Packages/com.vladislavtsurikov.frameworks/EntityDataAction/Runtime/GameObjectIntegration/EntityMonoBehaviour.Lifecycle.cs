namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public partial class EntityMonoBehaviour
    {
        protected void Update() => InvokeIfActive(action => action.InvokeUpdate());

        protected void FixedUpdate() => InvokeIfActive(action => action.InvokeFixedUpdate());

        protected void LateUpdate() => InvokeIfActive(action => action.InvokeLateUpdate());

        protected void OnEnable()
        {
            SetupEntityBindings();
            Entity.SetSetupData(new object[] { this });

            if (!Active)
            {
                return;
            }

            InvokeAwakeIfNeeded();
            Entity.Enable();
            InvokeStartIfNeeded();
        }

        protected void OnDisable()
        {
            if (_entity == null)
            {
                return;
            }

            Entity.Disable();
        }

        protected void OnDestroy()
        {
            if (_entity == null)
            {
                return;
            }

            ForEachLifecycleAction(action => action.InvokeOnDestroy());
        }

        protected void OnApplicationFocus(bool hasFocus) =>
            InvokeIfActive(action => action.InvokeOnApplicationFocus(hasFocus));

        protected void OnApplicationPause(bool pauseStatus) =>
            InvokeIfActive(action => action.InvokeOnApplicationPause(pauseStatus));

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (_entity == null)
            {
                return;
            }

            ForEachLifecycleAction(action => action.InvokeOnValidate());
        }
#endif
    }
}
