using System;
using UnityEngine;

namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public abstract class EntityMonoBehaviourAction : EntityLifecycleAction
    {
        protected EntityMonoBehaviour Host { get; private set; }

        protected sealed override void OnFirstSetupComponent(object[] setupData = null)
        {
            Host = null;
            if (setupData != null)
            {
                for (int i = 0; i < setupData.Length; i++)
                {
                    if (setupData[i] is EntityMonoBehaviour host)
                    {
                        Host = host;
                        break;
                    }
                }
            }
            OnFirstSetupComponentWithHost(setupData);
        }

        protected virtual void OnFirstSetupComponentWithHost(object[] setupData = null)
        {
        }

        protected TComponent[] GetComponentsInChildren<TComponent>(bool includeInactive) where TComponent : MonoBehaviour
        {
            if (Host == null)
            {
                return Array.Empty<TComponent>();
            }

            return Host.GetComponentsInChildren<TComponent>(includeInactive);
        }

    }
}
