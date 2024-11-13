using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pudding.StateMachine.ScriptableObjects
{
	[Serializable]
	public abstract class StateActionSO : DescriptionSMActionBaseSO
	{
		internal StateAction GetAction(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (createdInstances.TryGetValue(this, out var obj))
				return (StateAction)obj;

			var action = CreateAction();
			createdInstances.Add(this, action);
			action._originSO = this;
			action.Awake(stateMachine);
			return action;
		}

        internal StateAction GetAction(InteractiveObject interactiveObject, GameObject owner)
        {
            var action = CreateAction();
			action.Awake(interactiveObject, owner);
            action._originSO = this;
            return action;
        }
        protected abstract StateAction CreateAction();
	}

	public abstract class StateActionSO<T> : StateActionSO where T : StateAction, new()
	{
		protected override StateAction CreateAction() => new T();
	}
}
