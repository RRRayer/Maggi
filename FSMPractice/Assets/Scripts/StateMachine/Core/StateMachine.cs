using System;
using System.Collections.Generic;
using UnityEngine;

namespace Test.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [Tooltip("Set the initial state of this StateMachine")]
        [SerializeField] private ScriptableObjects.TransitionTableSO _transitionTableSO = default;

        private readonly Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
        internal State _currentState = default;

        private void Awake()
        {
            // State Machine √ ±‚»≠
            _currentState = _transitionTableSO.GetInitialState(this);
        }

        private void Start()
        {
            _currentState.OnStateEnter();
        }

        private void Update()
        {
            if (_currentState.TryGetTransition(out var transitionState))
                Transition(transitionState);

            _currentState.OnUpdate();
        }

        private void FixedUpdate()
        {
            if (_currentState.TryGetTransition(out var transitionState))
                Transition(transitionState);

            _currentState.OnFixedUpdate();
        }

        private void Transition(State transitionState)
        {
            _currentState.OnStateExit();
            _currentState = transitionState;
            _currentState.OnStateEnter();
        }
    }
}

