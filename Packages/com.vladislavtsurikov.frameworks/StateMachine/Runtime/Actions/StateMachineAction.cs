using System.Linq;
using UnityEngine;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.StateMachine.Runtime.Data;
using VladislavTsurikov.StateMachine.Runtime.Definitions;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.StateMachine.Runtime.Actions
{
    [Name("StateMachine/StateMachineAction")]
    public sealed class StateMachineAction : EntityLifecycleAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private StateMachineData _data;

        protected override void OnEnable()
        {
            _data = Get<StateMachineData>();

            if (_data.CurrentState == null)
            {
                EvaluateConditions();
            }

            _data.ActiveStatesChanged += EvaluateConditions;

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    var current = _data.CurrentState;
                    if (current == null)
                    {
                        return;
                    }

                    current.Update(Entity, Time.deltaTime);
                })
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions?.Clear();
            if (_data != null)
            {
                _data.ActiveStatesChanged -= EvaluateConditions;
                for (int i = 0; i < _data.StateStack.ElementList.Count; i++)
                {
                    var state = _data.StateStack.ElementList[i];
                    if (state == null)
                    {
                        continue;
                    }

                    state.IsEligibleForTransition = false;
                }

                _data = null;
            }
        }

        public void EvaluateConditions()
        {
            var entity = Entity;
            var data = Get<StateMachineData>();
            if (entity == null || data == null)
            {
                return;
            }

            var best = data.ActiveStates
                .Where(state => state != null && !ReferenceEquals(state, data.CurrentState))
                .OrderByDescending(state => state.Priority)
                .FirstOrDefault();

            TrySwitchState(best);
        }

        public bool TrySwitchState(State nextState)
        {
            var data = Get<StateMachineData>();
            if (data == null || nextState == null)
            {
                return false;
            }

            if (ReferenceEquals(data.CurrentState, nextState))
            {
                return false;
            }

            var current = data.CurrentState;
            if (current != null && !current.CanExit(Entity, nextState))
            {
                return false;
            }

            if (!nextState.CanEnter(Entity, current))
            {
                return false;
            }

            current?.Exit(Entity);
            data.PreviousState = current;
            data.CurrentState = nextState;
            nextState.Enter(Entity);
            return true;
        }
    }
}
