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
        private ReactiveProperty<bool> _isEligibleForTransition = new ReactiveProperty<bool>();

        [NonSerialized]
        private CompositeDisposable _subscriptions = new CompositeDisposable();

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

        public ReactiveProperty<bool> IsEligibleForTransition
        {
            get
            {
                _isEligibleForTransition ??= new ReactiveProperty<bool>();
                return _isEligibleForTransition;
            }
            set => _isEligibleForTransition = value;
        }

        protected override void SetupComponent(object[] setupData = null)
        {
            _subscriptions = new CompositeDisposable();

            Entity = (EntityMonoBehaviour)setupData[0];
            _owner = (StateMachineData)setupData[1];

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
                IsEligibleForTransition.Value = false;
                return;
            }

            eligibleStream
                .DistinctUntilChanged()
                .Subscribe(active =>
                {
                    IsEligibleForTransition.Value = active;
                    _owner?.SetStateEligible(this, active);
                })
                .AddTo(_subscriptions);
        }
    }
}
