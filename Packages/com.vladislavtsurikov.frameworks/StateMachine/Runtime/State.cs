using System;
using OdinSerializer;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.StateMachine.Runtime.Data;

namespace VladislavTsurikov.StateMachine.Runtime.Definitions
{
    [Serializable]
    public class State : Node
    {
        [OdinSerialize]
        private string _stateId;
        [OdinSerialize]
        private int _priority;

        [NonSerialized]
        private StateMachineData _owner;

        [OdinSerialize, HideInInspector]
        private ReactiveProperty<bool> _active = new ReactiveProperty<bool>();

        [NonSerialized]
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected CompositeDisposable Subscriptions => _subscriptions;

        public EntityMonoBehaviour Entity { get; private set; }

        public string StateId
        {
            get => _stateId;
            set
            {
                if (_stateId == value)
                {
                    return;
                }

                _stateId = value;
            }
        }

        public int Priority
        {
            get => _priority;
            set
            {
                if (_priority == value)
                {
                    return;
                }

                _priority = value;
            }
        }

        public bool IsEligibleForTransition
        {
            get => _active.Value;
            set
            {
                if (_active.Value == value)
                {
                    return;
                }

                _active.Value = value;
                _owner?.SetStateEligible(this, value);
            }
        }

        public IReadOnlyReactiveProperty<bool> IsEligibleForTransitionReactive => _active;

        protected override void SetupComponent(object[] setupData = null)
        {
            Entity = setupData[0] as EntityMonoBehaviour;
            _owner = setupData[1] as StateMachineData;

            Conditional();
        }

        protected override void OnDisableElement()
        {
            _subscriptions?.Clear();
        }

        protected T GetData<T>() where T : ComponentData
        {
            return Entity?.GetData<T>();
        }

        protected T GetAction<T>() where T : EntityAction
        {
            return Entity?.GetAction<T>();
        }

        public virtual bool CanEnter(Entity entity, State fromState) => true;
        public virtual bool CanExit(Entity entity, State toState) => true;
        public virtual void Enter(Entity entity) { }
        public virtual void Exit(Entity entity) { }
        public virtual void Update(Entity entity, float deltaTime) { }
        protected virtual void Conditional() { }

        protected void BindEligibility(Func<bool> predicate)
        {
            if (predicate == null)
            {
                BindEligibility((IObservable<bool>)null);
                return;
            }

            BindEligibility(Observable.EveryUpdate().Select(_ => predicate()));
        }

        protected void BindEligibility(IObservable<bool> eligibleStream)
        {
            _subscriptions.Clear();

            if (eligibleStream == null)
            {
                IsEligibleForTransition = false;
                return;
            }

            eligibleStream
                .DistinctUntilChanged()
                .Subscribe(active => IsEligibleForTransition = active)
                .AddTo(_subscriptions);
        }
    }
}
