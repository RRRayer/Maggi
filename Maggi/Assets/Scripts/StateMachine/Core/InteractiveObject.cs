using UnityEngine;
using System.Collections.Generic;

namespace Pudding.StateMachine.ScriptableObjects
{
    public enum InteractionType
    {
        None, General, NonPossession, Possession,
    }

    public abstract class InteractiveObject : MonoBehaviour
    {
        public InteractionType m_Type = InteractionType.General;

        public StateActionSO[] m_IdleScriptableActions;
        public StateActionSO[] m_WalkScriptableActions;
        public StateActionSO[] m_JumpAscendingScriptableActions;
        public StateActionSO[] m_JumpDescendingScriptableActions;
        public StateActionSO[] m_PullScriptableActions;
        public StateActionSO[] m_PushScriptableActions;

        private StateAction[] _idleActions = null;
        private StateAction[] _walkActions = null;
        private StateAction[] _jumpAscendingActions = null;
        private StateAction[] _jumpDescendingActions = null;
        private StateAction[] _pullActions = null;
        private StateAction[] _pushActions = null;

        private void Awake()
        {
            SetStateActions(m_IdleScriptableActions, ref _idleActions);
            SetStateActions(m_WalkScriptableActions, ref _walkActions);
            SetStateActions(m_JumpAscendingScriptableActions, ref _jumpAscendingActions);
            SetStateActions(m_JumpDescendingScriptableActions, ref _jumpDescendingActions);
            SetStateActions(m_PullScriptableActions, ref _pullActions);
            SetStateActions(m_PushScriptableActions, ref _pushActions);
        }

        private void SetStateActions(StateActionSO[] scriptableActions, ref StateAction[] actions)
        {
            if (scriptableActions != null && scriptableActions.Length > 0)
            {
                var stateMachine = new StateMachine();
                var createdInstances = new Dictionary<ScriptableObject, object>();

                actions = new StateAction[scriptableActions.Length];
                for (int i = 0; i < scriptableActions.Length; i++)
                {
                    actions[i] = scriptableActions[i].GetAction();
                }
            }
        }

        #region IDLE
        public virtual void OnIdleStateEnter()
        {
            if (_idleActions == null) return;

            for (int i = 0; i < _idleActions.Length; ++i)
            {
                Debug.Log(_idleActions[i]);
                _idleActions[i].OnStateEnter();
            }
        }

        public virtual void OnIdleStateExit()
        {
            if (_idleActions == null) return;

            for (int i = 0; i < _idleActions.Length; ++i)
                _idleActions[i].OnStateExit();
        }

        public virtual void OnIdleUpdate()
        {
            if (_idleActions == null) return;

            for (int i = 0; i < _idleActions.Length; i++)
                _idleActions[i].OnUpdate();
        }
        #endregion

        #region WALK
        public virtual void OnWalkStateEnter()
        {
            if (_walkActions == null) return;

            for (int i = 0; i < _walkActions.Length; ++i)
                _walkActions[i].OnStateEnter();
        }

        public virtual void OnWalkStateExit()
        {
            if (_walkActions == null) return;

            for (int i = 0; i < _walkActions.Length; ++i)
                _walkActions[i].OnStateExit();
        }

        public virtual void OnWalkUpdate()
        {
            if (_walkActions == null) return;

            for (int i = 0; i < _walkActions.Length; i++)
                _walkActions[i].OnUpdate();
        }
        #endregion

        #region PULL
        public virtual void OnPullStateEnter()
        {
            if (_pullActions == null) return;

            for (int i = 0; i < _pullActions.Length; ++i)
                _pullActions[i].OnStateEnter();
        }

        public virtual void OnPullStateExit()
        {
            if (_pullActions == null) return;

            for (int i = 0; i < _pullActions.Length; ++i)
                _pullActions[i].OnStateExit();
        }

        public virtual void OnPullUpdate()
        {
            if (_pullActions == null) return;

            for (int i = 0; i < _pullActions.Length; i++)
                _pullActions[i].OnUpdate();
        }
        #endregion

        #region PUSH
        public virtual void OnPushStateEnter()
        {
            if (_pushActions == null) return;

            for (int i = 0; i < _pushActions.Length; ++i)
                _pushActions[i].OnStateEnter();
        }

        public virtual void OnPushStateExit()
        {
            if (_pushActions == null) return;

            for (int i = 0; i < _pushActions.Length; ++i)
                _pushActions[i].OnStateExit();
        }

        public virtual void OnPushUpdate()
        {
            if (_pushActions == null) return;

            for (int i = 0; i < _pushActions.Length; i++)
                _pushActions[i].OnUpdate();
        }
        #endregion

        #region JUMP_ASCENDING
        public virtual void OnJumpAscendingStateEnter()
        {
            if (_jumpAscendingActions == null) return;

            for (int i = 0; i < _jumpAscendingActions.Length; ++i)
                _jumpAscendingActions[i].OnStateEnter();
        }

        public virtual void OnJumpAscendingStateExit()
        {
            if (_jumpAscendingActions == null) return;

            for (int i = 0; i < _jumpAscendingActions.Length; ++i)
                _jumpAscendingActions[i].OnStateExit();
        }

        public virtual void OnJumpAscendingUpdate()
        {
            if (_jumpAscendingActions == null) return;

            for (int i = 0; i < _jumpAscendingActions.Length; i++)
                _jumpAscendingActions[i].OnUpdate();
        }
        #endregion

        #region JUMP_DESCENDING
        public virtual void OnJumpDescendingStateEnter()
        {
            if (_jumpDescendingActions == null) return;

            for (int i = 0; i < _jumpDescendingActions.Length; ++i)
                _jumpDescendingActions[i].OnStateEnter();
        }

        public virtual void OnJumpDescendingStateExit()
        {
            if (_jumpDescendingActions == null) return;

            for (int i = 0; i < _jumpDescendingActions.Length; ++i)
                _jumpDescendingActions[i].OnStateExit();
        }

        public virtual void OnJumpDescendingUpdate()
        {
            if (_jumpDescendingActions == null) return;

            for (int i = 0; i < _jumpDescendingActions.Length; i++)
                _jumpDescendingActions[i].OnUpdate();
        }
        #endregion

    }
}
