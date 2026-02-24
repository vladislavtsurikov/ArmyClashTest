using System;
using System.Collections.Generic;
using OdinSerializer;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace VladislavTsurikov.StateMachine.Runtime.Data
{
    [Name("StateMachine/StateMachineData")]
    public sealed class StateMachineData : EntityComponentData
    {
        [OdinSerialize]
        private NodeStackOnlyDifferentTypes<State> _stateStack = new NodeStackOnlyDifferentTypes<State>();

        [OdinSerialize, HideInInspector]
        private ReactiveProperty<State> _currentState = new();

        [OdinSerialize, HideInInspector]
        private State _previousState;

        [NonSerialized]
        private readonly List<State> _activeStates = new List<State>();

        public event Action ActiveStatesChanged;

        public State CurrentState
        {
            get => _currentState.Value;
            set
            {
                if (_currentState.Value == value)
                {
                    return;
                }

                _previousState = _currentState.Value;

                NodeStackOnlyDifferentTypes<State> stack = StateStack;
                foreach (State state in stack.ElementList)
                {
                    state.Active = false;
                }

                _currentState.Value = value;
                value.Active = true;

                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<State> CurrentStateReactive => _currentState;

        public State PreviousState
        {
            get => _previousState;
            set
            {
                if (_previousState == value)
                {
                    return;
                }

                _previousState = value;
                MarkDirty();
            }
        }

        public NodeStackOnlyDifferentTypes<State> StateStack => _stateStack;

        public IReadOnlyList<State> ActiveStates => _activeStates;

        protected override void SetupComponent(object[] setupData = null)
        {
            _stateStack.Setup(true, new object[]{Entity, this});
        }

        protected override void OnDisableElement()
        {
            _stateStack.OnDisable();
        }

        internal void SetStateEligible(State state, bool eligible)
        {
            if (state == null)
            {
                return;
            }

            if (eligible)
            {
                if (!_activeStates.Contains(state))
                {
                    _activeStates.Add(state);
                    ActiveStatesChanged?.Invoke();
                }
            }
            else
            {
                if (_activeStates.Remove(state))
                {
                    ActiveStatesChanged?.Invoke();
                }
            }
        }
    }
}
