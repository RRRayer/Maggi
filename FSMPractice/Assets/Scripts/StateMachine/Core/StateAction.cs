using Test.StateMachine.ScriptableObjects;

namespace Test.StateMachine
{
	public abstract class StateAction : IStateComponent
	{
		internal StateActionSO _originSO;
		protected StateActionSO OriginSO => _originSO;

		public abstract void OnUpdate();
		public virtual void OnFixedUpdate() { }
		public virtual void Awake(StateMachine stateMachine) { }
		public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }

		public enum SpecificMoment
		{
			OnStateEnter, OnStateExit, OnUpdate,
		}
	}
}
