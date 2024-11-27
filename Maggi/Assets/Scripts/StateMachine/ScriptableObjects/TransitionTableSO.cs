using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maggi.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTransitionTable", menuName = "State Machines/Transition Table")]
    public class TransitionTableSO : ScriptableObject
    {
        [SerializeField] private TransitionItem[] _transitions = default;

        internal State GetInitialState(StateMachine stateMachine)
        {
            var states = new List<State>(); // FromStates들을 저장
            var transitions = new List<StateTransition>(); // 
            var createdInstances = new Dictionary<ScriptableObject, object>(); // 만들어진 Instances 저장용도(State들)

            var fromStates = _transitions.GroupBy(transition => transition.FromState);

            foreach (var fromState in fromStates)
            {
                if (fromState.Key == null)
                    throw new ArgumentNullException(nameof(fromState.Key), $"TransitionTable: {name}");

                // 만약 fromState가 Dictionary에 저장이 안되어 있으면 새로 State 만들어서 추가해줌
                var state = fromState.Key.GetState(stateMachine, createdInstances);
                states.Add(state);

                // FromState에 대한 모든 ToStates들로의 Trasitions을 뽑아내는 과정
                transitions.Clear();
                foreach (var transitionItem in fromState)
                {
                    if (transitionItem.ToState == null)
                        throw new ArgumentNullException(nameof(transitionItem.ToState), $"TransitionTable: {name}, From State: {fromState.Key.name}");

                    var toState = transitionItem.ToState.GetState(stateMachine, createdInstances);
                    ProcessConditionUsages(stateMachine, transitionItem.Conditions, createdInstances, out var conditions, out var resultGroups);
                    transitions.Add(new StateTransition(toState, conditions, resultGroups));
                }
                state._transitions = transitions.ToArray();
            }

            return states.Count > 0 ? states[0]
                : throw new InvalidOperationException($"TransitionTable {name} is empty.");
        }

        private static void ProcessConditionUsages(
            StateMachine stateMachine,
            ConditionUsage[] conditionUsages,
            Dictionary<ScriptableObject, object> createdInstances,
            out StateCondition[] conditions,
            out int[] resultGroups)
        {
            int count = conditionUsages.Length; // from 에서 to 로 가는 conditions 의 수

            // 한 transition에 대한 conditions들을 모두 뽑아내는 과정
            conditions = new StateCondition[count];
            for (int i = 0; i < count; i++)
                conditions[i] = conditionUsages[i].Condition.GetCondition(stateMachine, conditionUsages[i].ExpectedResult == Result.True, createdInstances);

            // 이거는 뭐지?? resultGroupsList의 정체가 뭐냔 말이야 !!!!!
            List<int> resultGroupsList = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int idx = resultGroupsList.Count;
                resultGroupsList.Add(1);
                while (i < count - 1 && conditionUsages[i].Operator == Operator.And)
                {
                    i++;
                    resultGroupsList[idx]++;
                }
            }

            resultGroups = resultGroupsList.ToArray();
        }

        [Serializable]
        public struct TransitionItem
        {
            public StateSO FromState;
            public StateSO ToState;
            public ConditionUsage[] Conditions;
        }

        [Serializable]
        public struct ConditionUsage
        {
            public Result ExpectedResult;
            public StateConditionSO Condition;
            public Operator Operator;
        }

        public enum Result { True, False }
        public enum Operator { And, Or }
    }
}
