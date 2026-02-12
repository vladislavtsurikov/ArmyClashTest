using System;
using VladislavTsurikov.Nody.Runtime.Core;
using UnityEngine;
using Action = VladislavTsurikov.ActionFlow.Runtime.Actions.Action;

namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public abstract class EntityAction : Action
    {
        public Entity Entity
        {
            get
            {
                EntityActionCollection collection = Stack as EntityActionCollection;
                if (collection == null)
                {
                    return null;
                }

                return collection.Entity;
            }
        }

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

        protected virtual void OnValidate()
        {
        }

        protected virtual void OnApplicationFocus(bool hasFocus)
        {
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
        }

        protected T Get<T>() where T : ComponentData
        {
            if (Entity == null)
            {
                return null;
            }

            return (T)Entity.Data.GetElement(typeof(T));
        }

        protected TComponent[] GetComponentsInChildren<TComponent>(bool includeInactive) where TComponent : MonoBehaviour
        {
            if (Entity == null)
            {
                return Array.Empty<TComponent>();
            }

            return Entity.GetComponentsInChildren<TComponent>(includeInactive);
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

        internal void InvokeOnValidate()
        {
            if (Active)
            {
                OnValidate();
            }
        }

        internal void InvokeOnApplicationFocus(bool hasFocus)
        {
            if (Active)
            {
                OnApplicationFocus(hasFocus);
            }
        }

        internal void InvokeOnApplicationPause(bool pauseStatus)
        {
            if (Active)
            {
                OnApplicationPause(pauseStatus);
            }
        }
    }
}
