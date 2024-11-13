using Pudding.StateMachine.ScriptableObjects;
using System;
using UnityEngine;

namespace Pudding.StateMachine
{
	public abstract class StateAction : IStateComponent
	{
		internal StateActionSO _originSO;
		protected StateActionSO OriginSO => _originSO;

		public abstract void OnUpdate();
		public virtual void OnFixedUpdate() { }
		public virtual void Awake(StateMachine stateMachine) { }
        public virtual void Awake(InteractiveObject interactiveObject, GameObject owner) { }
        public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }

		public enum SpecificMoment
		{
			OnStateEnter, OnStateExit, OnUpdate,
		}
	}
}
