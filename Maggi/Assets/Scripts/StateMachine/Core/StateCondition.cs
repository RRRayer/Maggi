using Maggi.StateMachine.ScriptableObjects;

namespace Maggi.StateMachine
{
    public abstract class Condition : IStateComponent
    {
        private bool _isCached = false;
        private bool _cachedStatement = default;

        internal StateConditionSO _originSO;
        protected StateConditionSO OriginSO => _originSO;

        protected abstract bool Statement();

        public virtual void Awake(StateMachine stateMachine) { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }

        internal bool GetStatement()
        {
            if (!_isCached)
            {
                _isCached = true;
                _cachedStatement = Statement();
            }

            return _cachedStatement;
        }

        internal void ClearStatementCache()
        {
            _isCached = false;
        }
    }

    public struct StateCondition
    {
        internal StateMachine _stateMachine;
        internal Condition _condition;
        internal bool _expectedResult;

        public StateCondition(StateMachine stateMachine, Condition condition, bool expectedResult)
        {
            _stateMachine = stateMachine;
            _condition = condition;
            _expectedResult = expectedResult;
        }

        public bool IsMet()
        {
            bool statement = _condition.GetStatement();
            bool isMet = statement == _expectedResult;

            return isMet;
        }
    }
}

