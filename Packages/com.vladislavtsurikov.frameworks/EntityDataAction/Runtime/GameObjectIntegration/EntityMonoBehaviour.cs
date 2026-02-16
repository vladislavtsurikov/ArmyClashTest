using System;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using UnityEngine;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    [ExecuteInEditMode]
    public partial class EntityMonoBehaviour : SerializedMonoBehaviour
    {
        [OdinSerialize]
        private Entity _entity;
        private bool _actionsAwakeCalled;
        private bool _actionsStartCalled;

        public Entity Entity
        {
            get
            {
                EnsureEntity();
                return _entity;
            }
        }

        public EntityDataCollection Data => Entity.Data;
        public EntityActionCollection Actions => Entity.Actions;

        public bool IsSetup => Entity.IsSetup;
        public DirtyActionRunner DirtyRunner => Entity.DirtyRunner;

        public bool LocalActive
        {
            get => Entity.LocalActive;
            set => Entity.LocalActive = value;
        }

        public bool GlobalActive
        {
            get => Entity.GlobalActive;
            set => Entity.GlobalActive = value;
        }

        public bool IsEntityActive => Active;

        internal bool Active
        {
            get
            {
#if UNITY_EDITOR
                if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
                {
                    return false;
                }
#endif

                return Entity.Active;
            }
        }

        protected virtual void OnSetupEntity()
        {
            if (Active)
            {
                Entity.Data.Setup();
                Entity.Actions.Setup();

                Entity.Data.ElementAdded += HandleDataChanged;
                Entity.Data.ElementRemoved += HandleDataChanged;

                Entity.Actions.Run().Forget();
            }
        }

        protected virtual Type[] ComponentDataTypesToCreate()
        {
            return null;
        }

        protected virtual Type[] ActionTypesToCreate()
        {
            return null;
        }

        protected virtual void OnAfterCreateDataAndActions()
        {
        }

        public T GetData<T>() where T : ComponentData
        {
            return Entity.GetData<T>();
        }

        public T GetAction<T>() where T : EntityAction
        {
            return Entity.GetAction<T>();
        }

        public void Setup()
        {
            Entity.Setup();
        }

        protected void HandleDataChanged(int index)
        {
            Entity.HandleDataChanged(index);
        }

        private void EnsureEntity()
        {
            if (_entity == null)
            {
                _entity = new Entity();
            }
        }

        private void SetupEntityBindings()
        {
            Entity.ComponentDataTypesProvider = ComponentDataTypesToCreate;
            Entity.ActionTypesProvider = ActionTypesToCreate;
            Entity.AfterCreateDataAndActionsCallback = _ => OnAfterCreateDataAndActions();
            Entity.SetupEntityCallback = _ => OnSetupEntity();
        }

        private void InvokeAwakeIfNeeded()
        {
            if (_actionsAwakeCalled)
            {
                return;
            }

            _actionsAwakeCalled = true;
            ForEachLifecycleAction(action => action.InvokeAwake());
        }

        private void InvokeStartIfNeeded()
        {
            if (_actionsStartCalled)
            {
                return;
            }

            _actionsStartCalled = true;
            ForEachLifecycleAction(action => action.InvokeStart());
        }

        private void InvokeIfActive(Action<EntityLifecycleAction> handler)
        {
            if (!Active)
            {
                return;
            }

            ForEachLifecycleAction(handler);
        }

        private void ForEachLifecycleAction(Action<EntityLifecycleAction> handler)
        {
            if (handler == null)
            {
                return;
            }

            var actions = Entity.Actions;
            if (actions == null)
            {
                return;
            }

            for (int i = 0; i < actions.ElementList.Count; i++)
            {
                if (actions.ElementList[i] is EntityLifecycleAction action)
                {
                    handler(action);
                }
            }
        }
    }
}
