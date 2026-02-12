using System;
using UnityEngine;

namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public abstract class EntityMonoBehaviourAction : EntityAction
    {
        protected EntityMonoBehaviour Host { get; private set; }

        protected sealed override void OnFirstSetupComponent(object[] setupData = null)
        {
            Host = FindHost(setupData);
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

        private static EntityMonoBehaviour FindHost(object[] setupData)
        {
            if (setupData == null)
            {
                return null;
            }

            for (int i = 0; i < setupData.Length; i++)
            {
                if (setupData[i] is EntityMonoBehaviour host)
                {
                    return host;
                }
            }

            return null;
        }
    }
}
