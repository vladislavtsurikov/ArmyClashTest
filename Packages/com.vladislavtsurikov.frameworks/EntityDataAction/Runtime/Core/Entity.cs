using System;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public partial class Entity
    {
        [OdinSerialize]
        private EntityDataCollection _data;
        [OdinSerialize]
        private EntityActionCollection _actions;
        [OdinSerialize]
        private bool _localActive = true;

        private object[] _setupData;

        internal DirtyActionRunner DirtyRunner;

        internal Func<Type[]> ComponentDataTypesProvider;
        internal Func<Type[]> ActionTypesProvider;
        internal Action<Entity> AfterCreateDataAndActionsCallback;
        internal Action<Entity> SetupEntityCallback;

        public EntityDataCollection Data => _data;
        public EntityActionCollection Actions => _actions;
        public object[] SetupData => _setupData;

        public bool IsSetup { get; private set; }

        public bool LocalActive
        {
            get => _localActive;
            set
            {
                if (_localActive == value)
                {
                    return;
                }

                _localActive = value;
                HandleActiveChanged();
            }
        }

        public bool GlobalActive
        {
            get => EntityDataActionGlobalSettings.Active;
            set => EntityDataActionGlobalSettings.Active = value;
        }

        public bool Active => _localActive && EntityDataActionGlobalSettings.Active;

        public void SetSetupData(object[] setupData)
        {
            _setupData = setupData;
        }

        protected virtual void OnSetupEntity()
        {
            if (Active)
            {
                _data.Setup(true, _setupData);
                _actions.Setup(true, _setupData);

                _data.ElementAdded += HandleDataChanged;
                _data.ElementRemoved += HandleDataChanged;

                _actions.Run().Forget();
            }
        }

        public void Setup()
        {
            if (IsSetup)
            {
                return;
            }

            EntityDataActionGlobalSettings.ActiveChanged -= HandleActiveChanged;
            EntityDataActionGlobalSettings.ActiveChanged += HandleActiveChanged;

            _data ??= new EntityDataCollection();
            _data.Entity = this;
            _actions ??= new EntityActionCollection();
            _actions.Entity = this;

            DirtyRunner ??= new DirtyActionRunner(this, _data, _actions);
            DirtyRunner.Setup();

            CreateDefaultData();
            CreateDefaultActions();

            AfterCreateDataAndActionsCallback?.Invoke(this);
            OnAfterCreateDataAndActions();

            if (SetupEntityCallback != null)
            {
                SetupEntityCallback(this);
            }
            else
            {
                OnSetupEntity();
            }

            IsSetup = true;
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
            return Data.GetElement<T>();
        }

        public T GetAction<T>() where T : EntityAction
        {
            return Actions.GetElement<T>();
        }

        internal void HandleDataChanged(int index)
        {
            DirtyRunner?.TriggerAll();
        }

        private void CreateDefaultData()
        {
            Type[] types = ComponentDataTypesProvider != null
                ? ComponentDataTypesProvider()
                : ComponentDataTypesToCreate();

            if (types == null)
            {
                return;
            }

            _data.SyncToTypes(types);
        }

        private void CreateDefaultActions()
        {
            if (_actions == null)
            {
                return;
            }

            Type[] types = ActionTypesProvider != null
                ? ActionTypesProvider()
                : ActionTypesToCreate();

            if (types == null)
            {
                return;
            }

            _actions.SyncToTypes(types);
        }

        public void Enable()
        {
            Setup();

            if (!Active)
            {
                return;
            }

            InvokeOnEnable();
        }

        public void Disable()
        {
            InvokeOnDisable();

            EntityDataActionGlobalSettings.ActiveChanged -= HandleActiveChanged;
            _data.ElementAdded -= HandleDataChanged;
            _data.ElementRemoved -= HandleDataChanged;

            _data.OnDisable();
            _actions.OnDisable();

            DirtyRunner?.OnDisable();

            IsSetup = false;
        }

        private void HandleActiveChanged()
        {
            if (_localActive && EntityDataActionGlobalSettings.Active)
            {
                Disable();
                Enable();
            }
            else
            {
                Disable();
            }
        }

        private void InvokeOnEnable() => ForEachAction(action => action.InvokeOnEnable());
        private void InvokeOnDisable() => ForEachAction(action => action.InvokeOnDisable());

        private void ForEachAction(Action<EntityAction> handler)
        {
            if (handler == null || _actions == null)
            {
                return;
            }

            for (int i = 0; i < _actions.ElementList.Count; i++)
            {
                if (_actions.ElementList[i] is EntityAction action)
                {
                    handler(action);
                }
            }
        }
    }
}
