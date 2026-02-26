namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public abstract class EntityLifecycleAction : EntityAction
    {
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        protected virtual void LateUpdate()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        internal void InvokeAwake()
        {
            if (Active)
            {
                Awake();
            }
        }

        internal void InvokeStart()
        {
            if (Active)
            {
                Start();
            }
        }

        internal void InvokeUpdate()
        {
            if (Active)
            {
                Update();
            }
        }

        internal void InvokeFixedUpdate()
        {
            if (Active)
            {
                FixedUpdate();
            }
        }

        internal void InvokeLateUpdate()
        {
            if (Active)
            {
                LateUpdate();
            }
        }

        internal void InvokeOnEnable()
        {
            if (Active)
            {
                OnEnable();
            }
        }

        internal void InvokeOnDisable()
        {
            if (Active)
            {
                OnDisable();
            }
        }

        internal void InvokeOnDestroy()
        {
            if (Active)
            {
                OnDestroy();
            }
        }
    }
}
