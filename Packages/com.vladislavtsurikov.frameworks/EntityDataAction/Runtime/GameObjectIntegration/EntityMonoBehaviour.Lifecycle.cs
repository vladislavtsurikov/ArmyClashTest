namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public partial class EntityMonoBehaviour
    {
        protected void OnEnable()
        {
            EnsureEntity();
            SetupEntityBindings();
            Entity.SetSetupData(new object[] { this });

            if (!Active)
            {
                return;
            }

            Entity.Enable();
        }

        protected void OnDisable()
        {
            if (_entity == null)
            {
                return;
            }

            Entity.Disable();
        }

        protected void Update()
        {
            if (!Active)
            {
                return;
            }

            Entity.Update();
        }

        protected void FixedUpdate()
        {
            if (!Active)
            {
                return;
            }

            Entity.FixedUpdate();
        }

        protected void LateUpdate()
        {
            if (!Active)
            {
                return;
            }

            Entity.LateUpdate();
        }

        protected void OnDestroy()
        {
            _entity?.Destroy();
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            _entity?.Validate();
        }
#endif

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (!Active)
            {
                return;
            }

            Entity.OnApplicationFocus(hasFocus);
        }

        protected void OnApplicationPause(bool pauseStatus)
        {
            if (!Active)
            {
                return;
            }

            Entity.OnApplicationPause(pauseStatus);
        }
    }
}
